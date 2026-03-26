using System;
using System.Collections;
using System.Collections.Generic;
using Advanced_Audio_Sources.scripts.AudioProfiles;
using UnityEngine;
using Random = System.Random;


namespace Advanced_Audio_Sources.scripts.AudioSources
{
    [RequireComponent(typeof(AudioSource))]
    public class AdvancedAudioSource : MonoBehaviour
    {
        // References
        private AudioSource _audioSource;
        
        // Constants
        private const int Decimals = 2;
        private float _decimalWorth = Mathf.Pow(10, Decimals);
        
        private const HideFlags HideFlag = HideFlags.HideInInspector;
        
        
        // Variables
        [Header("Engine Settings")]
        [SerializeField] private AudioProfile audioProfile;
        public AudioProfile AudioProfile { get => audioProfile; set { audioProfile = value; SetSourceValues(); } }
        [SerializeField] private bool hideAudioComponent = true;
        public bool HideAudioComponent { get => hideAudioComponent; set { hideAudioComponent = value; SetSourceValues(); } }
        
        [Header("Clips")]
        [SerializeField] private List<AudioClip> clips;
        [SerializeField] private bool mute = false;
        public bool Mute { get => mute; set { mute = value; SetSourceValues(); } }
        
        [Header("Basic Settings")]
        [SerializeField] private bool playOnAwake = true;
        public bool PlayOnAwake { get => playOnAwake; set { playOnAwake = value; SetSourceValues(); } }
        [SerializeField] private bool looping = false;
        public bool Looping { get => looping; set { looping = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 256)] private int priority = 128;
        public int Priority { get => priority; set { priority = value; SetSourceValues(); } }
        [SerializeField] [Range(-1, 1)] private float stereoPan = 0;
        public float StereoPan { get => stereoPan; set { stereoPan = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 1)] private float spatialBlend = 0;
        public float SpatialBlend { get => spatialBlend; set { spatialBlend = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 1.1f)] private float reverbZoneMix = 1;
        public float ReverbZoneMix { get => reverbZoneMix; set { reverbZoneMix = value; SetSourceValues(); } }
        
        [Header("Listener Settings")]
        [SerializeField] private bool ignorePause = false;
        public bool IgnorePause { get => ignorePause; set { ignorePause = value; SetSourceValues(); } }
        [SerializeField] private bool ignoreListenerVolume = false;
        public bool IgnoreListenerVolume { get => ignoreListenerVolume; set { ignoreListenerVolume = value; SetSourceValues(); } }
        
        // Randomization
        [Header("Randomization")]
        [SerializeField][Range(-3, 3)] private float basePitch = 1f;
        public float BasePitch { get => basePitch; set { basePitch = value; SetSourceValues(); } }
        [SerializeField][Range(-3, 3)] private float randomPitch = 1f;
        public float RandomPitch { get => randomPitch; set { randomPitch = value; SetSourceValues(); } }
        
        [SerializeField][Range(0, 1)] private float baseVolume = 1f;
        private float _currentBaseVolume = 1f;
        public float BaseVolume { get => baseVolume; set { baseVolume = value; SetSourceValues(); } }
        [SerializeField][Range(0, 1)] private float randomVolume = 1f;
        public float RandomVolume { get => randomVolume; set { randomVolume = value; SetSourceValues(); } }
        
        
        // Set up
        private void Awake() { 
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.hideFlags = HideFlag;
            
            SetSourceValues();
        }

        private void Start() { if (playOnAwake == true) PlayAudio(false);
        }

        private void OnValidate() { 
            SetSourceValues();
            
            // Hide/ show component
            GetComponent<AudioSource>().hideFlags = hideAudioComponent == true ? HideFlag : HideFlags.None;
        }
        private void SetSourceValues()
        {
            if (mute == true) playOnAwake = false;
            
            if (_audioSource != null)
            {
                _audioSource.mute = mute;
                
                _audioSource.loop = looping;
                _audioSource.priority = priority;
                _audioSource.panStereo = stereoPan;
                _audioSource.spatialBlend = spatialBlend;
                _audioSource.reverbZoneMix = reverbZoneMix;
                
                _audioSource.ignoreListenerPause = ignorePause;
                _audioSource.ignoreListenerVolume = ignoreListenerVolume;
            }

            var newVolume = baseVolume;
            if (audioProfile != null) newVolume = audioProfile.CalculateVolume(baseVolume);
            _currentBaseVolume = newVolume;
        }


        // Play audio
        public void PlayAudio(bool playOnce)
        {
            var random = new Random();
            
            // Pick an audio clip
            var number = random.Next(0, clips.Count);
            var usedClip = clips[number];
            _audioSource.clip = usedClip;
            
            // Pick a random pitch
            _audioSource.pitch = RandomNumber(random, randomPitch, basePitch);
            
            // Pick a random volume
            _audioSource.volume = RandomNumber(random, randomVolume, _currentBaseVolume);
            
            // Play sound
            if (playOnce == true) _audioSource.PlayOneShot(usedClip);
            else _audioSource.Play();
        }
        
        // Random number
        private float RandomNumber(Random generator, float min, float max)
        {
            var minValue = (int)(min * _decimalWorth);
            var maxValue = (int)(max * _decimalWorth);
            var randNumber = ((min < max) ? generator.Next(minValue, maxValue) : generator.Next(maxValue, minValue)) / _decimalWorth;
            
            return randNumber;
        }
        
        
        
        // Play delayed audio
        public void PlayDelayedAudio(bool playOnce, float time) { StartCoroutine(PlayDelayedCoroutine(playOnce, time)); }
        
        private IEnumerator PlayDelayedCoroutine(bool playOnce, float time)
        {
            var waitTime = new WaitForSeconds(time);
            yield return waitTime;
            
            PlayAudio(playOnce);
            
            yield break;
        }
        
        
        // Pausing the player
        private void Pause(bool pausing)
        {
            Action method = pausing ? _audioSource.Pause : _audioSource.UnPause;
            method.Invoke();
        }
        
        
        // Misc functions
        private void OnDestroy() { StopAllCoroutines(); }
        private void OnDisable() { StopAllCoroutines(); }
    }
}
    
