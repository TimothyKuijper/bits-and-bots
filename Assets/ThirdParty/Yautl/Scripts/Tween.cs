using UnityEngine;
using System.Linq;
using System;

namespace Yakanashe.Yautl
{
    [Serializable]
    public class Tween<T> : ITween
    {
        public float Duration { get; private set; }
        public float ElapsedTime { get; private set; }
        public int LoopCount { get; private set; }
        public object Owner { get; }
        public bool LoopPingPong { get; private set; }

        public float Progress => Mathf.Clamp01(ElapsedTime / Duration);
        public float TimeLeft => Duration - ElapsedTime;
        public bool IsComplete => ElapsedTime >= Duration;

        private readonly EaseType _easeType;
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;
        private readonly T _startValue;
        private readonly T _endValue;
        private readonly Func<T, T, float, T> _lerpFunc;

        private event Action _onComplete;
        private event Action<float> _onUpdate;
        private bool _ignoreTimescale;
        private bool _paused;
        private bool _completed;
        private bool _pingPongReverse;
        private int _loopsDone;

        public Tween(object owner, Func<T> getter, Action<T> setter, T to, float duration, EaseType easeType, Func<T, T, float, T> lerpFunc)
        {
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
            _setter = setter ?? throw new ArgumentNullException(nameof(setter));
            _lerpFunc = lerpFunc ?? throw new ArgumentNullException(nameof(lerpFunc));
            Duration = duration;
            _easeType = easeType;
            Owner = owner;

            _startValue = _getter();
            _endValue = to;
            ElapsedTime = 0f;
        }

        /// <summary>
        /// Starts playing this tween
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Play()
        {
            TweenRunner.Instance.Run(this);
            return this;
        }

        /// <summary>
        /// Stops running this tween
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Stop()
        {
            TweenRunner.Instance.Remove(this);
            return this;
        }

        /// <summary>
        /// Skips to the end of this loop, and continues if there are any loops left
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Skip()
        {
            ElapsedTime = Duration;
            Refresh();
            Complete();
            return this;
        }

        /// <summary>
        /// Directly sets the progress of this tween at the current loop
        /// </summary>
        /// <param name="progress"></param>
        /// <returns>This tween</returns>
        public ITween SetProgress(float progress)
        {
            ElapsedTime = Mathf.Clamp01(progress) * Duration;
            Refresh();
            if (IsComplete) Complete();
            return this;
        }

        /// <summary>
        /// Skips to the end of this loop, and stops it
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Finish()
        {
            Skip();
            Stop();
            return this;
        }

        /// <summary>
        /// Sets the progress in the current loop and stops the tween
        /// </summary>
        /// <param name="progress"></param>
        /// <returns>This tween</returns>
        public ITween FinishAt(float progress)
        {
            SetProgress(progress);
            Stop();
            return this;
        }

        /// <summary>
        /// Pauses this tween
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Pause()
        {
            _paused = true;
            return this;
        }

        /// <summary>
        /// Resumes this tween
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Resume()
        {
            _paused = false;
            return this;
        }

        /// <summary>
        /// Makes it so this tween runs independent of the Time.timeScale
        /// </summary>
        /// <returns>This tween</returns>
        public ITween IgnoreTimeScale()
        {
            _ignoreTimescale = true;
            return this;
        }

        /// <summary>
        /// Directly sets the loops for this tween, set it to -1 to loop infinitely
        /// </summary>
        /// <param name="count">How many times this tween will loop</param>
        /// <param name="pingPong">Makes this tween ping pong between the start and end value instead of snapping to the start value each loop</param>
        /// <returns>This tween</returns>
        public ITween SetLoops(int count, bool pingPong = false)
        {
            LoopCount = count;
            LoopPingPong = pingPong;
            return this;
        }

        /// <summary>
        /// Resets this tween to the start value
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Reset()
        {
            _setter(_startValue);
            return this;
        }

        /// <summary>
        /// Updates this tweens ElapsedTime based on if Timescale is ignored
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Update()
        {
            if (_completed || _paused) return this;

            ElapsedTime += _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;

            Refresh();

            if (ElapsedTime >= Duration) Complete();

            return this;
        }

        /// <summary>
        /// Adds a single tween to play after this one completes
        /// </summary>
        /// <returns>This tween</returns>
        public ITween Chain(ITween nextTween)
        {
            OnComplete(() => nextTween.Play());
            return this;
        }

        /// <summary>
        /// Plays a list of tweens at once when this one ends
        /// </summary>
        /// <returns>This tween</returns>
        public ITween ChainParralel(params ITween[] nextTweens)
        {
            foreach (var next in nextTweens)
            {
                OnComplete(() => next.Play());
            }

            return this;
        }

        /// <summary>
        /// Plays a list of tweens one after another when this one ends
        /// </summary>
        /// <returns>This tween</returns>
        public ITween ChainSequential(params ITween[] nextTweens)
        {
            var list = nextTweens?.ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                var current = list[i];
                var next = list[i + 1];
                current.OnComplete(() => next.Play());
            }

            return this;
        }

        /// <summary>
        /// Adds to the onComplete event for this tween
        /// </summary>
        /// <param name="onComplete">Action to add</param>
        /// <returns>This tween</returns>
        public ITween OnComplete(Action onComplete)
        {
            _onComplete += onComplete;
            return this;
        }

        /// <summary>
        /// Adds to the onUpdate event for this tween
        /// </summary>
        /// <param name="onUpdate">Action to add</param>
        /// <returns>This tween</returns>
        public ITween OnUpdate(Action<float> onUpdate)
        {
            _onUpdate += onUpdate;
            return this;
        }

        private void Refresh()
        {
            var eased = _easeType.Evaluate(Progress);

            var from = _pingPongReverse ? _endValue : _startValue;
            var to = _pingPongReverse ? _startValue : _endValue;

            _setter(_lerpFunc(from, to, eased));
            _onUpdate?.Invoke(Progress);
        }

        private void Complete()
        {
            _loopsDone++;
            if (_loopsDone < LoopCount || LoopCount < 0)
            {
                ElapsedTime = 0f;
                if (LoopPingPong) _pingPongReverse = !_pingPongReverse;
                return;
            }

            _onComplete?.Invoke();
            _completed = true;
        }
    }
}