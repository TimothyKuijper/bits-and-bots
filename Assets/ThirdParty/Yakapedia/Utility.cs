using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yakapedia
{
    public static class Utility
    {
        /// <summary>
        /// Destroys all child objects from a parent object.
        /// </summary>
        /// <param name="parent">Parent object to remove all child objects from.</param>
        public static void DestroyAllChildren(this Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Finds the closest object of type T to the specified position.
        /// </summary>
        /// <typeparam name="T">The type of MonoBehaviour to search for.</typeparam>
        /// <param name="position">The position to measure the distance from.</param>
        /// <returns>The closest object of type T to the specified position, or null if none are found.</returns>
        public static T GetClosestObjectOfType<T>(this Transform position) where T : MonoBehaviour
        {
            var objectsOfType = UnityEngine.Object.FindObjectsOfType<T>();
            if (objectsOfType.Length == 0) return null;

            var closest = objectsOfType[0];
            var closestDistance = Vector2.Distance(position.position, closest.transform.position);
            for (int i = 1; i < objectsOfType.Length; i++)
            {
                float distance = Vector2.Distance(position.position, objectsOfType[i].transform.position);
                if (!(distance < closestDistance))
                {
                    continue;
                }

                closest = objectsOfType[i];
                closestDistance = distance;
            }

            return closest;
        }

        /// <summary>
        /// Retrieves all components of type T found in the GameObject hierarchy under the given root GameObject.
        /// </summary>
        /// <typeparam name="T">The type of component to search for.</typeparam>
        /// <param name="parent">The root GameObject to search under.</param>
        /// <returns>An array of components of type T found in the GameObject hierarchy under the root.</returns>
        public static T[] GetAllComponentsUnderParent<T>(this GameObject parent)
        {
            return parent.GetComponentsInChildren<Transform>().Select(transform => transform.GetComponent<T>()).Where(component => component != null).ToArray();
        }

        /// <summary>
        /// Shuffles the elements in the given array into a random order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to be shuffled.</param>
        public static void ShuffleArray<T>(ref IEnumerable<T> array)
        {
            var enumerable = array.ToList();
            for (int i = enumerable.Count - 1; i > 0; i--)
            {
                var j = UnityEngine.Random.Range(0, i + 1);
                (enumerable[i], enumerable[j]) = (enumerable[j], enumerable[i]);
            }

            array = enumerable;
        }

        /// <summary>
        /// Adds a unique item to the array if it's not already present.
        /// </summary>
        /// <typeparam name="T">The type of items in the array.</typeparam>
        /// <param name="array">The original array.</param>
        /// <param name="itemToAdd">The item to add.</param>
        /// <returns>A new array with the added item if it was not already present; otherwise, the original array.</returns>
        public static void AddUnique<T>(ref IEnumerable<T> array, T itemToAdd)
        {
            var list = array.ToList();
            if (list.Contains(itemToAdd))
            {
                return;
            }

            list.Add(itemToAdd);
            array = list.AsEnumerable();
        }


        /// <summary>
        /// Rotates the transform to look at the target
        /// </summary>
        /// <param name="transform">The transform to rotate</param>
        /// <param name="target">The target point to look at</param>
        public static void LookAt2D(this Transform transform, Vector2 target)
        {
            var direction = target - (Vector2)transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        /// <summary>
        /// Randomizes 2D rotation (from 0 to 360 degrees).
        /// </summary>
        /// <param name="transform">The transform to randomize rotation on.</param>
        public static void RandomizeRotation2D(this Transform transform)
        {
            transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
        }

        /// <summary>
        /// Randomizes rotation.
        /// </summary>
        /// <param name="transform">The transform to randomize rotation on.</param>
        public static void RandomizeRotation(this Transform transform)
        {
            transform.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
        }

        /// <summary>
        /// Generates a random direction as a Vector2 with the specified magnitude.
        /// </summary>
        /// <param name="magnitude">The magnitude of the generated direction vector.</param>
        /// <returns>A random direction vector with the specified magnitude.</returns>
        public static Vector2 RandomDirection(float magnitude = 1f)
        {
            var angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            return direction * magnitude;
        }

        /// <summary>
        /// Takes a screenshot of the screen and saves it into a custom folder in the persistentDataPath.
        /// The screenshot filename includes the current date and the product name (title of the game).
        /// </summary>
        /// <param name="screenSizeMultiplier">How many times bigger the screenshot is then the current screen resolution.</param>
        public static void TakeScreenshot(int screenSizeMultiplier = 1)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, "Yakapedia", "Screenshots");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filename = Path.Combine(folderPath, Application.productName + "_Screenshot_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
            ScreenCapture.CaptureScreenshot(filename, screenSizeMultiplier);
            Debug.Log("Screenshot saved to: " + filename);
        }

        /// <summary>
        /// Just returns the date, not the time.
        /// </summary>
        /// <returns>Date</returns>
        public static string GetDate()
        {
            return DateTime.Now.ToString("dd-MM-yyyy");
        }

        /// <summary>
        /// Just returns the time, not the date.
        /// </summary>
        /// <returns>Time</returns>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Checks if the user's finger is currently touching any UI element on a specific layer.
        /// </summary>
        /// <param name="layerName">The name of the layer to check against.</param>
        /// <returns>True if the finger is touching a UI element on the specified layer; otherwise, false.</returns>
        public static bool IsFingerTouchingLayer(string layerName)
        {
            if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began)
            {
                return false;
            }

            var touch = Input.GetTouch(0);
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = touch.position
            };

            var uiRaycasters = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>();
            foreach (var raycaster in uiRaycasters)
            {
                if (raycaster.gameObject.layer != LayerMask.NameToLayer(layerName)) continue;
                var results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the cursor position is currently touching any UI element on a specific layer.
        /// </summary>
        /// <param name="layerName">The name of the layer to check against.</param>
        /// <returns>True if the cursor is touching a UI element on the specified layer; otherwise, false.</returns>
        public static bool IsCursorTouchingLayer(string layerName)
        {
            var touch = Input.mousePosition;
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = touch
            };
            var uiRaycasters = UnityEngine.Object.FindObjectsOfType<GraphicRaycaster>();
            foreach (var raycaster in uiRaycasters)
            {
                if (raycaster.gameObject.layer != LayerMask.NameToLayer(layerName)) continue;
                var results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Loads a scene based on a relative index offset.
        /// </summary>
        /// <param name="offset">The relative index offset to determine the scene to load.
        ///     Positive values load scenes ahead in the build index.
        ///     Negative values load scenes before in the build index.
        ///     Zero reloads the current scene.</param>
        public static void LoadSceneWithRelativeOffset(int offset = 0)
        {
            if (SceneManager.GetActiveScene().buildIndex + offset < 0 || SceneManager.GetActiveScene().buildIndex + offset > SceneManager.sceneCount)
            {
                Debug.LogWarning("Scene buildIndex out of bounds");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + offset);
            }
        }

        /// <summary>
        /// Executes onTimerComplete after duration seconds.
        /// </summary>
        /// <param name="duration">How many seconds before onTimerComplete is executed</param>
        /// <param name="onTimerComplete">The method to execute after duration seconds</param>
        /// <returns></returns>
        public static IEnumerator TimedAction(float duration, Action onTimerComplete)
        {
            yield return new WaitForSeconds(duration);
            onTimerComplete?.Invoke();
        }

        /// <summary>
        /// Executes onTimerComplete after duration seconds.
        /// </summary>
        /// <param name="duration">How many seconds before onTimerComplete is executed</param>
        /// <param name="onTimerComplete">The method to execute after duration seconds</param>
        public static IEnumerator TimedUnityEvent(float duration, UnityEvent onTimerComplete)
        {
            yield return new WaitForSeconds(duration);
            onTimerComplete?.Invoke();
        }

        /// <summary>
        /// Tries to get the specified component from the GameObject. If the component is not found, 
        /// it adds the component to the GameObject and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the component to get or add.</typeparam>
        /// <param name="gameObject">The GameObject to search for or add the component to.</param>
        /// <returns>The existing or newly added component of the specified type.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

#if UNITY_EDITOR
        [MenuItem("Yakapedia/Clear Undo History")]
        public static void ClearUndoHistory()
        {
            Undo.ClearAll();
        }

        /// <summary>
        /// Creates a generic singleton of a class (must inherit from this one)
        /// </summary>
        /// <typeparam name="T">Class to make singleton of (will most likely be the same as the class that inherits this one)</typeparam>
        public class GenericSingleton<T> : MonoBehaviour where T : Component
        {
            private static T _instance;

            public static T Instance
            {
                get
                {
                    if (_instance != null) return _instance;

                    _instance = FindObjectOfType<T>();
                    if (_instance != null) return _instance;

                    var container = new GameObject(typeof(T).Name);
                    _instance = container.AddComponent<T>();

                    return _instance;
                }
            }
        }

        /// <summary> Renames the variable in the inspector </summary>
        public class Rename : PropertyAttribute
        {
            public string InspectorName { get; }

            public Rename(string name)
            {
                InspectorName = name;
            }
        }

        [CustomPropertyDrawer(typeof(Rename))]
        public class RenameDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.PropertyField(position, property, new GUIContent((attribute as Rename).InspectorName));
            }
        }
#endif
    }
}