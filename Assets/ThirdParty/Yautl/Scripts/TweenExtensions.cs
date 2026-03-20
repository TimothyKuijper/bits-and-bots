using UnityEngine.UI;
using UnityEngine;

namespace Yakanashe.Yautl
{
    public static class TweenExtensions
    {
        public static ITween To(this float start, float end, float duration, EaseType ease = EaseType.InOutSine)
        {
            var tween = new Tween<float>(null, () => start, value => start = value, end, duration, ease, Mathf.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }

        public static ITween MoveTo(this Transform transform, Vector3 to, float duration, EaseType ease = EaseType.InOutSine)
        {
            var tween = new Tween<Vector3>(transform, () => transform.position, value => transform.position = value, to, duration, ease, Vector3.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }

        public static ITween ScaleTo(this Transform transform, Vector3 to, float duration, EaseType ease = EaseType.InOutSine)
        {
            var tween = new Tween<Vector3>(transform, () => transform.localScale, v => transform.localScale = v, to, duration, ease, Vector3.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }

        public static ITween RotateTo(this Transform transform, Vector3 eulerTo, float duration, EaseType ease = EaseType.InOutSine)
        {
            var tween = new Tween<Vector3>(transform, () => transform.eulerAngles, v => transform.eulerAngles = v, eulerTo, duration, ease, Vector3.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }

        public static ITween ColorTo(this Graphic graphic, Color to, float duration, EaseType ease = EaseType.InOutSine)
        {
            var tween = new Tween<Color>(graphic.transform, () => graphic.color, v => graphic.color = v, to, duration, ease, Color.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }

        public static ITween ColorTo(this Renderer renderer, Color to, float duration, EaseType ease = EaseType.InOutSine)
        {
            var mat = renderer.material;
            var tween = new Tween<Color>(renderer.transform, () => mat.color, v => mat.color = v, to, duration, ease, Color.Lerp);
            TweenRunner.Instance.Run(tween);
            return tween;
        }
    }
}