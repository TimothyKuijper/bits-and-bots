using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using System.IO;
using System;

namespace Yakapedia
{
    public static class PersistentData
    {
        private static string CurrentSaveFile => Path.Combine(Application.persistentDataPath, "Yakapedia", "Saves", Application.productName.Replace(' ', '_') + "_SaveFile_" + (PlayerPrefs.HasKey("SelectedSave") ? PlayerPrefs.GetInt("SelectedSave") : 0) + ".bin");

        private static string GetSaveFilePath(int index)
        {
            return Path.Combine(Application.persistentDataPath, "Yakapedia", "Saves", Application.productName.Replace(' ', '_') + "_SaveFile_" + index + ".bin");
        }

        private static readonly string JsonEncryptionKey = GenerateDeviceSpecificKey();
        private static readonly object _locker = new();

        private static Dictionary<string, object> _values;

        /// <summary>
        /// Saves a value into the currently selected save file.
        /// </summary>
        /// <typeparam name="T">Type to set</typeparam>
        /// <param name="key">The key for the data entry.</param>
        /// <param name="value">The value to set.</param>
        public static void Set<T>(string key, T value = default)
        {
            if (_values == null)
            {
                GetDictionary();
            }

            _values[key] = value;
            Save();
        }

        /// <summary>
        /// Returns a value corresponding to key in the currently selected save file if it exists.
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <param name="key">The key for the data entry.</param>
        /// <param name="defaultValue">The default value to return if there is no entry using the key.</param>
        public static T Get<T>(string key, T defaultValue = default)
        {
            if (_values == null)
            {
                GetDictionary();
            }

            if (_values.TryGetValue(key, out var val))
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }

            Debug.Log($"Dictionary doesn't contain the key: {key}");

            return defaultValue;
        }

        /// <summary>
        /// Returns a value corresponding to key in the currently selected save file if it exists. Uses JsonUtility to convert instead of straight converting.
        /// </summary>
        /// <typeparam name="T">Type to get</typeparam>
        /// <param name="key">The key for the data entry.</param>
        /// <param name="defaultValue">The default value to return if there is no entry using the key.</param>
        public static T GetSerialized<T>(string key, T defaultValue = default)
        {
            if (_values == null)
            {
                GetDictionary();
            }

            if (_values.TryGetValue(key, out var val))
            {
                JsonUtility.FromJsonOverwrite(val.GetType() == typeof(string)
                    ? val.ToString() : JsonUtility.ToJson(val), defaultValue);
                return defaultValue;
            }

            Debug.Log($"Dictionary doesn't contain the key: {key}");

            return defaultValue;
        }

        /// <summary>
        /// Checks if the key exists in the currently selected save file.
        /// </summary>
        /// <param name="key">The key for the data entry.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public static bool HasKey(string key)
        {
            if (_values == null)
            {
                GetDictionary();
            }

            return _values.ContainsKey(key);
        }

        /// <summary>
        /// Deletes the data entry with the specified key.
        /// </summary>
        /// <param name="key">The key of the data entry to delete.</param>
        public static void DeleteKey(string key)
        {
            if (_values == null)
            {
                GetDictionary();
            }

            _values.Remove(key);
            Save();
        }

        /// <summary>
        /// Select a save file, if it does not exist it will be made. This also reloads the current scene so all variables can be properly loaded in.
        /// </summary>
        /// <param name="index">Index of the save file that needs to be loaded</param>
        public static void SelectSaveFile(int index)
        {
            Debug.Log($"Selected save file with index: {index}");

            PlayerPrefs.SetInt("SelectedSave", index);
            GetDictionary();

            Utility.ReloadCurrentScene();
        }

        /// <summary>
        /// Deletes a save file based on index, and then loads another save file, which by default is 0.
        /// </summary>
        /// <param name="index">Index number of the save file to delete.</param>
        /// <param name="saveToLoad">Which save to load once the other one is deleted.</param>
        public static void DeleteSaveFile(int index, int saveToLoad = 0)
        {
            if (File.Exists(GetSaveFilePath(index)))
            {
                File.Delete(GetSaveFilePath(index));
            }

            Debug.Log($"Deleted save file with index: {index}");
            SelectSaveFile(saveToLoad);
        }

        /// <summary>
        /// Deletes the current save file.
        /// </summary>
        /// <param name="saveToLoad">Which save to load once the current one is deleted.</param>
        public static void DeleteCurrentSaveFile(int saveToLoad = 0)
        {
            if (File.Exists(CurrentSaveFile))
            {
                File.Delete(CurrentSaveFile);
            }

            Debug.Log("Deleted current save file");
            SelectSaveFile(saveToLoad);
        }

        /// <summary>
        /// Checks if a save file is empty or does not exist at all.
        /// </summary>
        /// <param name="index">The index of the save file to check</param>
        /// <returns>True if the save file does not exist or is empty; otherwise, false.</returns>
        public static bool IsSaveFileEmpty(int index)
        {
            if (!File.Exists(GetSaveFilePath(index))) return true;

            var data = File.ReadAllBytes(GetSaveFilePath(index));
            return data.Length == 0;
        }

        /// <summary>
        /// Saves all local (not session persistent) data to the currently selected save file.
        /// </summary>
        public static void Save()
        {
            if (!File.Exists(CurrentSaveFile))
            {
                var fi = new FileInfo(CurrentSaveFile);
                Directory.CreateDirectory(fi.Directory.FullName);
            }

            lock (_locker)
            {
                var jsonData = JsonConvert.SerializeObject(_values).StringToByte();
                var encryptedData = Encryption.Encrypt(jsonData, JsonEncryptionKey);

                File.WriteAllBytes(CurrentSaveFile, encryptedData);
            }
        }

        private static string GenerateDeviceSpecificKey()
        {
            var deviceId = SystemInfo.deviceUniqueIdentifier;
            using var sha256 = SHA256.Create();

            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceId));
            var stringBuilder = new StringBuilder();

            foreach (var hashedByte in hashedBytes)
            {
                stringBuilder.Append(hashedByte.ToString("x2"));
            }

            var deviceSpecificKey = stringBuilder.ToString();

            deviceSpecificKey = deviceSpecificKey[..16];
            deviceSpecificKey = deviceSpecificKey.Insert(2, "-");
            deviceSpecificKey += "@";

            return deviceSpecificKey;
        }

        private static void GetDictionary()
        {
            _values = new Dictionary<string, object>();
            if (!File.Exists(CurrentSaveFile))
            {
                var fi = new FileInfo(CurrentSaveFile);
                Directory.CreateDirectory(fi.Directory.FullName);
            }

            lock (_locker)
            {
                if (!File.Exists(CurrentSaveFile))
                {
                    Debug.Log("Save file didn't exist so returned empty dictionary.");
                    return;
                }

                var encrpytedData = File.ReadAllBytes(CurrentSaveFile);
                var decryptedData = Encryption.Decrypt(encrpytedData, JsonEncryptionKey);
                var data = new Dictionary<string, object>();

                if (decryptedData != null)
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decryptedData.ByteToString());
                }

                _values = data;
            }
        }
    }
}