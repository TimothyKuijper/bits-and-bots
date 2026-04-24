using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Advanced_Audio_Sources.scripts.AudioProfiles
{
    [ExecuteAlways]
    [CreateAssetMenu(fileName = "NewAudioProfile", menuName = "Advanced Audio Sources/AudioProfiles/New AudioProfile")]
    public class AudioProfile : ScriptableObject
    {
        // List of all available profiles
        public static List<AudioProfile> Profiles = new List<AudioProfile>();
        
        // Settings
        [SerializeField] private String profileName = "default";
        public String ProfileName => profileName;
        
        [SerializeField] [Range(0, 1)] private float baseVolume = 1;
        public float BaseVolume => baseVolume;
        
        
        // Set up
        private void Start()
        {
            AddProfileInstance(this);
        }
        
        
        // Calculate volume with profile
        public float CalculateVolume(float volume)
        {
            float newVolume = (BaseVolume < volume ? baseVolume : volume); // Make an actual calculation later
            return newVolume;
        }


        // When value is changed in inspector
        private void OnValidate()
        {
            RemoveProfile();
            AddProfileInstance(this);
        }


        // Adds a profile instance to the list of current existing profiles
        private void AddProfileInstance(AudioProfile newProfile)
        {
            if (HasProfile(this.profileName) == true) return;
            
            Profiles.Add(this);
        }
        
        
        
        // Check if profile with name exists
        public bool HasProfile(String searchedProfile)
        {
            foreach (var p in Profiles)
            {
                if (p.profileName == searchedProfile) return true;
            }

            return false;
        }
        
        
        
        // Remove profile when deleted
        private void RemoveProfile()
        {
            if (Profiles.Count <= 0) return;
            
            for (int index = (Profiles.Count - 1); index > 0; index--)
            {
                if (Profiles[index] == this)
                {
                    Profiles.Remove(this); 
                    return;
                }
            }
        }
        
        private void OnDestroy() { RemoveProfile(); }
    }
}
