using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yakapedia
{
    public static class PersistentLogger
    {
        private static string LogFilePath
        {
            get
            {
                string logDirectory = Path.Combine(Application.persistentDataPath, "Yakapedia", "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                return Path.Combine(logDirectory, Utility.GetDate() + ".txt");
            }
        }

        /// <summary>
        /// Logs a message into a file.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void Log(string message)
        {
            var sceneName = SceneManager.GetActiveScene().name;
            var currentTime = Utility.GetTime();

            var logMessage = $"{currentTime} (Scene: {sceneName}, Version: {Application.version}) | {message}";

            using StreamWriter writer = new StreamWriter(LogFilePath, true);
            writer.WriteLine(logMessage);
            
            Debug.Log($"Message saved | {message}");
        }

        /// <summary>
        /// Retrieves all the logs from the Logs folder.
        /// </summary>
        /// <returns>string, string dictionary containing all the logs.</returns>
        public static Dictionary<string, string> GetAllLogs()
        {
            var logFiles = new Dictionary<string, string>();

            var logDirectory = Path.Combine(Application.persistentDataPath, "Yakapedia", "Logs");
            if (!Directory.Exists(logDirectory))
            {
                return logFiles;
            }

            var logFileNames = Directory.GetFiles(logDirectory, "*.txt");
            foreach (string fileName in logFileNames)
            {
                string fileDate = Path.GetFileNameWithoutExtension(fileName);
                string fileData = File.ReadAllText(fileName);
                logFiles[fileDate] = fileData;
            }

            return logFiles;
        }
    }
}