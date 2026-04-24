using UnityEngine;

namespace Yakapedia
{
    /// <summary>
    /// Only works on Android.
    /// </summary>
    public static class Vibration
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        private static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        private static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        /// <summary>
        /// Starts vibration
        /// </summary>
        public static void Start()
        {
#if UNITY_ANDROID
            vibrator.Call("vibrate");
#endif
        }

        /// <summary>
        /// Stops vibration
        /// </summary>
        public static void Stop()
        {
#if UNITY_ANDROID
            vibrator.Call("cancel");
#endif
        }

        /// <summary>
        /// Starts vibration for a fixed amount of milliseconds
        /// </summary>
        /// <param name="milliseconds">Amount of milliseconds to vibrate for</param>
        public static void Vibrate(long milliseconds)
        {
#if UNITY_ANDROID
            vibrator.Call("vibrate", milliseconds);
#endif
        }

        /// <summary>
        /// Starts vibration with a pattern, i really don't know how this works so i'm not going to explain further
        /// </summary>
        /// <param name="pattern">An array representing the pattern of vibration in milliseconds.</param>
        /// <param name="repeat">The number of times to repeat the vibration pattern.</param>
        public static void Vibrate(long[] pattern, int repeat)
        {
#if UNITY_ANDROID
            vibrator.Call("vibrate", pattern, repeat);
#endif
        }
    }
}