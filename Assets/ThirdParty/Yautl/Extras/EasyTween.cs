using System;
using UnityEngine;

namespace Yakanashe.Yautl
{
    public class EasyTween : MonoBehaviour
    {
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float duration;
        [SerializeField] private EaseType easeType;

        private ITween tween;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                tween = new Tween<Vector3>(transform, () => transform.position, value => transform.position = value, endPosition, duration, easeType, Vector3.Lerp);
                tween.OnComplete(OnTweenEnd);
                tween.SetLoops(-1, true);
                tween.Play();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                transform.MoveTo(endPosition, duration);
            }
        }

        private static void OnTweenEnd()
        {
            Debug.Log("Tween finished");
        }
    }
}