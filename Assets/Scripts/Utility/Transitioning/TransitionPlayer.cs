using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

[RequireComponent(typeof(Transition))]
public class TransitionPlayer : MonoBehaviour
{
    private EventSystem _eventSystem;
    private Transition _transition;

    private void Awake()
    {
        _eventSystem = FindObjectsByType<EventSystem>(FindObjectsSortMode.None)[0];
        _transition = GetComponent<Transition>();
        _transition.Out(.1f);
    }
    public void TransitionToScene(string nextScene)
    {
        _eventSystem.enabled = false;
        _transition.In(.1f, () => { SceneManager.LoadScene(nextScene); });
    }
    public void TransitionToScene(int nextScene)
    {
        _eventSystem.enabled = false;
        _transition.In(.1f, () => { SceneManager.LoadScene(nextScene); });
    }
}
