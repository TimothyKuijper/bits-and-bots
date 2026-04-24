using UnityEngine;

namespace Yakapedia
{
    public static class Math
    {
        /// <summary>
        /// Rounds the given value to the nearest multiple of the specified increment.
        /// </summary>
        /// <param name="value">The value to be rounded.</param>
        /// <param name="increment">The increment to round to.</param>
        /// <returns>The rounded value.</returns>
        public static float RoundToNearest(this float value, float increment)
        {
            return Mathf.Round(value / increment) * increment;
        }
        
        /// <summary>
        /// Clamps the given angle value between the specified minimum and maximum angles.
        /// </summary>
        /// <param name="angle">The angle to be clamped.</param>
        /// <param name="min">The minimum allowed angle.</param>
        /// <param name="max">The maximum allowed angle.</param>
        /// <returns>The clamped angle value.</returns>
        public static float ClampAngle(this float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360f);
            if (angle > 180f)
            {
                angle -= 360f;
            }
            return Mathf.Clamp(angle, min, max);
        }
        
        /// <summary>
        /// Determines a boolean value based on a probability between 0 and 1.
        /// </summary>
        /// <param name="probability">The probability percentage between 0 and 1.</param>
        /// <returns>True if the random value falls within the probability range; otherwise, false.</returns>
        public static bool Probability(float probability)
        {
            return probability switch
            {
                >= 1.0f => true,
                <= 0.0f => false,
                _ => Random.value < probability
            };
        }
    }
}