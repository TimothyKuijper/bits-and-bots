using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yakanashe.Yautl
{
    public class TweenRunner : MonoBehaviour
    {
        private static TweenRunner instance;
        private readonly List<ITween> activeTweens = new();

        public static TweenRunner Instance
        {
            get
            {
                if (instance != null) return instance;

                var gameObject = new GameObject("TweenRunner");
                instance = gameObject.AddComponent<TweenRunner>();
                DontDestroyOnLoad(gameObject);
                return instance;
            }
        }

        private void Update()
        {
            for (var i = activeTweens.Count - 1; i >= 0; i--)
            {
                var activeTween = activeTweens[i];
                activeTween.Update();
                if (!activeTween.IsComplete) continue;
                activeTweens.RemoveAt(i);
            }
        }

        public void Run(ITween tween)
        {
            activeTweens.Add(tween);
        }

        public void Remove(ITween tween)
        {
            activeTweens.Remove(tween);
        }

        public void KillAllFrom(Transform owner)
        {
            for (int i = activeTweens.Count - 1; i >= 0; i--)
            {
                var tween = activeTweens[i];

                if (ReferenceEquals(tween.Owner, owner))
                {
                    activeTweens.RemoveAt(i);
                }
            }
        }
    }
}