using UnityEngine;

// https://easings.net
namespace Yakanashe.Wiper
{
    public static class EaseManager
    {
        public static float Evaluate(float t, EaseType ease)
        {
            switch (ease)
            {
                case EaseType.Linear:
                    return t;
                case EaseType.InSine:
                    return 1 - Mathf.Cos((t * Mathf.PI) / 2);
                case EaseType.OutSine:
                    return Mathf.Sin((t * Mathf.PI) / 2);
                case EaseType.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
                case EaseType.InQuad:
                    return t * t;
                case EaseType.OutQuad:
                    return 1 - (1 - t) * (1 - t);
                case EaseType.InOutQuad:
                    return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
                case EaseType.InCubic:
                    return t * t * t;
                case EaseType.OutCubic:
                    return 1 - Mathf.Pow(1 - t, 3);
                case EaseType.InOutCubic:
                    return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
                case EaseType.InQuart:
                    return t * t * t * t;
                case EaseType.OutQuart:
                    return 1 - Mathf.Pow(1 - t, 4);
                case EaseType.InOutQuart:
                    return t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
                case EaseType.InQuint:
                    return t * t * t * t * t;
                case EaseType.OutQuint:
                    return 1 - Mathf.Pow(1 - t, 5);
                case EaseType.InOutQuint:
                    return t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
                case EaseType.InExpo:
                    return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
                case EaseType.OutExpo:
                    return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
                case EaseType.InOutExpo:
                    return t switch
                    {
                        0 => 0,
                        1 => 1,
                        _ => t < 0.5 ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2
                    };
                case EaseType.InCirc:
                    return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
                case EaseType.OutCirc:
                    return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
                case EaseType.InOutCirc:
                    return t < 0.5
                        ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
                        : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;
                case EaseType.InBack:
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1;
                    return c3 * t * t * t - c1 * t * t;
                case EaseType.OutBack:
                    const float c2 = 1.70158f;
                    const float c4 = c2 + 1;
                    return 1 + c4 * Mathf.Pow(t - 1, 3) + c2 * Mathf.Pow(t - 1, 2);
                case EaseType.InOutBack:
                    const float c5 = 1.70158f;
                    const float c6 = c5 * 1.525f;
                    return t < 0.5
                        ? (Mathf.Pow(2 * t, 2) * ((c6 + 1) * 2 * t - c6)) / 2
                        : (Mathf.Pow(2 * t - 2, 2) * ((c6 + 1) * (t * 2 - 2) + c6) + 2) / 2;
                case EaseType.InElastic:
                    const float c7 = 2 * Mathf.PI / 3;
                    return t switch
                    {
                        0 => 0,
                        1 => 1,
                        _ => -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * c7)
                    };
                case EaseType.OutElastic:
                    const float c8 = 2 * Mathf.PI / 3;
                    return t switch
                    {
                        0 => 0,
                        1 => 1,
                        _ => Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c8) + 1
                    };
                case EaseType.InOutElastic:
                    const float c9 = 2 * Mathf.PI / 4.5f;
                    return t switch
                    {
                        0 => 0,
                        1 => 1,
                        _ => t < 0.5 ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c9)) / 2 : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c9)) / 2 + 1
                    };
                case EaseType.InBounce:
                    return 1 - OutBounce(1 - t);
                case EaseType.OutBounce:
                    if (t < 1 / 2.75f)
                    {
                        return 7.5625f * t * t;
                    }

                    if (t < 2 / 2.75f)
                    {
                        t -= 1.5f / 2.75f;
                        return 7.5625f * t * t + 0.75f;
                    }

                    if (t < 2.5 / 2.75)
                    {
                        t -= 2.25f / 2.75f;
                        return 7.5625f * t * t + 0.9375f;
                    }

                    t -= 2.625f / 2.75f;
                    return 7.5625f * t * t + 0.984375f;
                case EaseType.InOutBounce:
                    return t < 0.5
                        ? (1 - OutBounce(1 - 2 * t)) / 2
                        : (1 + OutBounce(2 * t - 1)) / 2;
                default:
                    return t;
            }
        }

        private static float OutBounce(float t)
        {
            if (t < 1 / 2.75f)
            {
                return 7.5625f * t * t;
            }

            if (t < 2 / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }

            if (t < 2.5 / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }

            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }
    }
}