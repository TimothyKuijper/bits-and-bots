using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yakanashe.Wiper;

[RequireComponent(typeof(Transition))]
public class TransitionPlayer : MonoBehaviour
{
    private Transition _transition;

    private void Awake()
    {
        _transition = GetComponent<Transition>();
        SceneManager.sceneLoaded += (arg, arg1) => { if (_transition) _transition.Out(); };
    }
    public void TransitionToScene(string nextScene) => _transition.In(0, () => { SceneManager.LoadScene(nextScene); });
    public void TransitionToScene(int nextScene) => _transition.In(0, () => { SceneManager.LoadScene(nextScene); });
}
