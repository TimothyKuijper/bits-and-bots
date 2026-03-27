using System;
using System.Collections;
using System.Collections.Generic;
using Advanced_Audio_Sources.scripts.AudioProfiles;
using UnityEngine;

namespace Advanced_Audio_Sources.scripts.AudioSources
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSource : MonoBehaviour
    {
        // References
        private AudioSource _audioSource;
        private AudioSource _fadeSource;
        public static MusicSource Instance { get; private set; }
        
        // Constants
        private const HideFlags HideFlag = HideFlags.HideInInspector;
        
        // Settings
        [Header("Engine Settings")]
        [SerializeField] private AudioProfile audioProfile;
        public AudioProfile AudioProfile { get => audioProfile; set { audioProfile = value; SetSourceValues(); } }
        [SerializeField] private bool hideAudioComponent = true;
        public bool HideAudioComponent { get => hideAudioComponent; set { hideAudioComponent = value; SetSourceValues(); } }
        
        [Header("Singleton Settings")] 
        [SerializeField] private bool destroyOnLoad = false;
        
        // Variables
        [Header("Music Settings")]
        [SerializeField] private List<AudioClip> clips;
        [SerializeField] private int currentTrack = 0;
        public int CurrentTrack => currentTrack;
        
        [Header("Basic Settings")]
        [SerializeField] private bool playOnAwake = true;
        public bool PlayOnAwake { get => playOnAwake; set { playOnAwake = value; SetSourceValues(); } }
        [SerializeField] private bool looping = true;
        public bool Looping { get => looping; set { looping = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 256)] private int priority = 128;
        public int Priority { get => priority; set { priority = value; SetSourceValues(); } }
        [SerializeField] [Range(-1, 1)] private float stereoPan = 0;
        public float StereoPan { get => stereoPan; set { stereoPan = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 1)] private float spatialBlend = 0;
        public float SpatialBlend { get => spatialBlend; set { spatialBlend = value; SetSourceValues(); } }
        [SerializeField] [Range(0, 1.1f)] private float reverbZoneMix = 1;
        public float ReverbZoneMix { get => reverbZoneMix; set { reverbZoneMix = value; SetSourceValues(); } }
        
        [SerializeField][Range(-3, 3)] private float pitch = 1f;
        public float Pitch { get => pitch; set { pitch = value; SetSourceValues(); } }
        [SerializeField][Range(0, 1)] private float volume = 1f;
        public float Volume { get => volume; set { volume = value; SetSourceValues(); } }
        
        [Header("Crossfade Settings")]
        [SerializeField] private bool crossFade = true;
        public bool CrossFade { get => crossFade; set { crossFade = value; } }
        [SerializeField] private float crossFadeTime = 0.6f;
        public float CrossFadeTime { get => crossFadeTime; set { crossFadeTime = value; } }
        
        [Header("Listener Settings")]
        [SerializeField] private bool ignorePause = false;
        public bool IgnorePause { get => ignorePause; set { ignorePause = value; SetSourceValues(); } }
        [SerializeField] private bool ignoreListenerVolume = false;
        public bool IgnoreListenerVolume { get => ignoreListenerVolume; set { ignoreListenerVolume = value; SetSourceValues(); } }
        
        
        // Set up
        private void Awake() { 
            // Singleton
            SetSingle();
            if (destroyOnLoad == false) DontDestroyOnLoad(this.gameObject);
            
            // Audio source
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.hideFlags = HideFlag;
            
            _fadeSource = gameObject.AddComponent<AudioSource>();
            _fadeSource.hideFlags = HideFlag;
            
            // Setting values
            SetSourceValues(false);
        }
        
        public void SetSingle()
        {
            if (Instance != null && Instance != this) Destroy(this.gameObject);
            else Instance = this;
        }

        private void Start() { if (playOnAwake == true) PlayMusic(currentTrack, true); }

        private void OnValidate()
        {
            SetSourceValues();
            
            // Hide/ show component
            GetComponents<AudioSource>()[0].hideFlags = hideAudioComponent == true ? HideFlag : HideFlags.None;
        }
        private void SetSourceValues(bool playSong = true)
        {
            if (_audioSource != null)
            {
                _audioSource.loop = looping;
                _audioSource.priority = priority;
                _audioSource.panStereo = stereoPan;
                _audioSource.spatialBlend = spatialBlend;
                _audioSource.reverbZoneMix = reverbZoneMix;

                _audioSource.pitch = pitch;
                _audioSource.volume = volume;
                if (audioProfile != null) _audioSource.volume = audioProfile.CalculateVolume(volume); // Set audio based on audio profile
                
                _audioSource.ignoreListenerPause = ignorePause;
                _audioSource.ignoreListenerVolume = ignoreListenerVolume;
                
                if (playSong == true) PlayMusic(currentTrack, true);
            }
        }
        
        
        
        // Play Music
        public void PlayMusic(int trackNumber, bool restart = false, bool transition = false)
        {
            if (restart == false && trackNumber == currentTrack) return;
            if (trackNumber > clips.Count || trackNumber < 0) return;

            if (crossFade == true && _audioSource.clip != null)
            {
                StartCoroutine(CrossFadeNewSong(trackNumber, transition));
                return;
            }

            var playTime = _audioSource.timeSamples;
            
            _audioSource.clip = clips[trackNumber];
            _audioSource.Play();
            
            var canTransition = playTime > _audioSource.clip.samples && transition;
            _audioSource.timeSamples = canTransition ? 0 : playTime;
            
            currentTrack = trackNumber;
        }
        
        
        // Crossfade songs
        private IEnumerator CrossFadeNewSong(int trackNumber, bool transition)
        {
            var startVolume = volume;
            var playTime = _audioSource.timeSamples;
            var newClip = clips[trackNumber];

            var canTransition = playTime > newClip.samples && transition;
            
            // Set fade source
            _fadeSource.clip = _audioSource.clip;
            _fadeSource.timeSamples = playTime;
            
            _fadeSource.Play();
            
            // Set regular audioSource
            _audioSource.clip = newClip;
            _audioSource.timeSamples = canTransition ? 0 : playTime;
            
            _audioSource.Play();
            
            // Set track to disable restarting the source
            currentTrack = trackNumber;
            
            // Lerp volume
            float elapsedTime = 0;
            while (elapsedTime < crossFadeTime)
            {
                elapsedTime += Time.deltaTime;
                
                _audioSource.volume = Mathf.Lerp(0, startVolume, elapsedTime);
                _fadeSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime);

                if (elapsedTime > crossFadeTime)
                {
                    _audioSource.volume = startVolume;
                    _fadeSource.volume = 0;
                    
                    elapsedTime = crossFadeTime;
                }

                yield return null;
            }
            _fadeSource.Stop();
            
            StopAllCoroutines();
        }
    }
}
