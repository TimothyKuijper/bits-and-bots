using UnityEngine;

public class TickExample : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    private TickRunner _tickRunner;

    private void Start()
    {
        _tickRunner = GetComponent<TickRunner>();
        _tickRunner.OnAttack.AddListener(_ => { particleSystem.Play(); });
    }
}