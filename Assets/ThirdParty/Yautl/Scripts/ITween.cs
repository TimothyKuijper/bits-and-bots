using System;
using UnityEngine;

namespace Yakanashe.Yautl
{
    public interface ITween
    {
        float Duration { get; }
        float ElapsedTime { get; }
        float Progress => ElapsedTime / Duration;
        float TimeLeft => Duration - ElapsedTime;

        bool IsComplete { get; }
        bool LoopPingPong { get; }
        int LoopCount { get; }
        object Owner { get; }

        
        ITween Play();
        ITween Stop();
        ITween Skip();
        ITween SetProgress(float progress);
        ITween Finish();
        ITween FinishAt(float progress);
        ITween Pause();
        ITween Resume();
        ITween IgnoreTimeScale();
        ITween SetLoops(int count, bool pingPong = false);
        ITween Reset();
        ITween Update();
        ITween Chain(ITween nextTween);
        ITween ChainParralel(params ITween[] nextTweens);
        ITween ChainSequential(params ITween[] nextTweens);
        ITween OnComplete(Action onComplete);
        ITween OnUpdate(Action<float> onUpdate);
    }
}