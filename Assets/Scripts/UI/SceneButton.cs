using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Transition Options")]
    [SerializeField] private string sceneString;
    [SerializeField] private int sceneIndex;

    private void Start()
    {
        var button = GetComponent<Button>();

        if (sceneString != "")
        {
            button.onClick.AddListener(() => transitionPlayer.TransitionToScene(sceneString));
            return;
        }
        button.onClick.AddListener(() => transitionPlayer.TransitionToScene(sceneIndex));
    }
}
