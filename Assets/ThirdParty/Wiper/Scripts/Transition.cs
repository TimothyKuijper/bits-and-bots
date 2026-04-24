using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Yakanashe.Wiper
{
    public class Transition : MonoBehaviour
    {
        public Material TransitionMaterial;
        public TransitionCollection transitionCollection;
        
        [SerializeField, Range(0.1f, 5f)] private float transitionDuration = 0.6f;


        [HideInInspector] public float Angle;
        [HideInInspector] public int SelectedTransitionIndex;

        private static readonly int _progress = Shader.PropertyToID("_Progress");
        private static readonly int _direction = Shader.PropertyToID("_Direction");
        
#if UNITY_EDITOR
        private const string DefaultMaterialPath = "Assets/Yakanashe/Wiper/Materials/WiperMaterial.mat";
        private const string DefaultTransitionCollectionPath = "Assets/Yakanashe/Wiper/ScriptableObjects/DefaultTransitionCollection.asset";
#endif
        public void Out(float delay = 0f, Action onComplete = null, EaseType ease = EaseType.Linear)
        {
            TransitionMaterial.SetFloat(_progress, 0);
            StartCoroutine(ExecuteTransition(1, delay, onComplete, ease));
        }

        public void Out()
        {
            TransitionMaterial.SetFloat(_progress, 0);
            StartCoroutine(ExecuteTransition(1, 0, null, EaseType.Linear));
        }

        public void In(float delay = 0f, Action onComplete = null, EaseType ease = EaseType.Linear)
        {
            TransitionMaterial.SetFloat(_progress, 1);
            StartCoroutine(ExecuteTransition(0, delay, onComplete, ease)); 
        }
        
        public void In()
        {
            TransitionMaterial.SetFloat(_progress, 1);
            StartCoroutine(ExecuteTransition(0, 0, null, EaseType.Linear)); 
        }

        private IEnumerator ExecuteTransition(float targetValue, float delay, Action onComplete, EaseType ease)
        {
            if (TransitionMaterial == null) yield break;

            var selectedTransition = transitionCollection.transitions[SelectedTransitionIndex];
            if (selectedTransition.shader != null)
            {
                TransitionMaterial.shader = selectedTransition.shader;
                if (TransitionMaterial.HasProperty(_direction))
                {
                    TransitionMaterial.SetFloat(_direction, Angle);
                }
            }

            if (delay > 0f) yield return new WaitForSeconds(delay);

            var startValue = TransitionMaterial.GetFloat(_progress);
            var elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Clamp01(elapsedTime / transitionDuration);

                var dT = EaseManager.Evaluate(t, ease);

                var currentValue = Mathf.Lerp(startValue, targetValue, dT);
                TransitionMaterial.SetFloat(_progress, currentValue);

                yield return null;
            }

            TransitionMaterial.SetFloat(_progress, targetValue);

            onComplete?.Invoke();
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            TransitionMaterial = AssetDatabase.LoadAssetAtPath<Material>(DefaultMaterialPath);
            transitionCollection = AssetDatabase.LoadAssetAtPath<TransitionCollection>(DefaultTransitionCollectionPath);
        }
        #endif
    }
}