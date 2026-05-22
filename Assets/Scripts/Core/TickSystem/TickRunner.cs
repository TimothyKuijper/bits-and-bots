using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TickRunner : MonoBehaviour
{
    public UnityEvent<float> OnAttack;
    
    [SerializeField] private TickPattern tickPattern;
    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private float damageMultiplier = 1;
    
    private TickPattern _tickPattern;
    private Coroutine _runTickCoroutine;

    public void Run()
    {
        if (_runTickCoroutine != null) StopCoroutine(_runTickCoroutine);
        _runTickCoroutine = StartCoroutine(RunTickPattern());
    }

    public void Stop()
    {
        if (_runTickCoroutine != null) StopCoroutine(_runTickCoroutine);
    }

    private IEnumerator RunTickPattern()
    {
        foreach (var tick in tickPattern.Ticks)
        {
            OnAttack.Invoke(tick.Damage * damageMultiplier);
            yield return new WaitForSeconds(tick.Duration * speedMultiplier);
        }

        _runTickCoroutine = StartCoroutine(RunTickPattern());
    }
}