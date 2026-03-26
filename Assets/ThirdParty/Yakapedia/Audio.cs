using System.Collections;
using UnityEngine;

namespace Yakapedia
{
    public static class Audio
    {
        /// <summary>
        /// Sets the volume of all AudioSources in the scene.
        /// </summary>
        /// <param name="volume">The volume value to set (0 to 1).</param>
        public static void SetGlobalVolume(float volume)
        {
            var allSources = Object.FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in allSources)
            {
                source.volume = Mathf.Clamp01(volume);
            }
        }

        /// <summary>
        /// Stops all AudioSources in the scene.
        /// </summary>
        public static void StopAllAudioSources()
        {
            var allSources = Object.FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in allSources)
            {
                source.Stop();
            }
        }
        
        /// <summary>
        /// Converts a frequency in hertz to a pitch value.
        /// </summary>
        /// <param name="frequency">The frequency in hertz.</param>
        /// <returns>The pitch value corresponding to the frequency.</returns>
        public static float FrequencyToPitch(float frequency)
        {
            return Mathf.Log(frequency / 440f, 2f) * 12f;
        }

        /// <summary>
        /// Converts a pitch value to a frequency in hertz.
        /// </summary>
        /// <param name="pitch">The pitch value.</param>
        /// <returns>The frequency in hertz corresponding to the pitch value.</returns>
        public static float PitchToFrequency(float pitch)
        {
            return 440f * Mathf.Pow(2f, pitch / 12f);
        }

        /// <summary>
        /// Plays a random sound effect from an array of AudioClips using an AudioSource component.
        /// </summary>
        /// <param name="source">The AudioSource component to play the sound effect.</param>
        /// <param name="clips">The array of AudioClips to choose from.</param>
        public static void PlayRandomSoundEffect(this AudioSource source, AudioClip[] clips)
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning("The array of AudioClips is empty.");
                return;
            }

            int randomIndex = Random.Range(0, clips.Length);
            source.PlayOneShot(clips[randomIndex]);
        }

        /// <summary>
        /// Fades the volume of an AudioSource component over a specified duration.
        /// </summary>
        /// <param name="source">The AudioSource component to fade.</param>
        /// <param name="targetVolume">The target volume to fade to.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <param name="playBeforeFade">Plays the AudioSource before it begins fading the volume</param>
        public static IEnumerator FadeVolume(this AudioSource source, float targetVolume, float duration, bool playBeforeFade = false)
        {
            if (playBeforeFade)
            {
                source.Play();
            }

            float startVolume = source.volume;
            float startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = elapsedTime / duration;
                source.volume = Mathf.Lerp(startVolume, targetVolume, t);
                yield return null;
            }

            source.volume = targetVolume;
        }

        /// <summary>
        /// Fades the pitch of an AudioSource component over a specified duration.
        /// </summary>
        /// <param name="source">The AudioSource component to fade.</param>
        /// <param name="targetPitch">The target pitch to fade to.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <param name="playBeforeFade">Plays the AudioSource before it begins fading the pitch</param>
        public static IEnumerator FadePitch(this AudioSource source, float targetPitch, float duration, bool playBeforeFade = false)
        {
            if (playBeforeFade)
            {
                source.Play();
            }

            float startPitch = source.pitch;
            float startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = elapsedTime / duration;
                source.pitch = Mathf.Lerp(startPitch, targetPitch, t);
                yield return null;
            }

            source.pitch = targetPitch;
        }
    }
}