using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Yautl;

public class ArenaCamera : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private List<Transform> cameraPositions;

    private int _lastIndex = -1;
    private ITween _moveTween;
    private ITween _rotateTween;

    public void SetRandomPosition()
    {
        var index = Random.Range(0, cameraPositions.Count);

        while (index == _lastIndex)
        {
            index = Random.Range(0, cameraPositions.Count);
        }

        _lastIndex = index;
        var randomTransform = cameraPositions[index];

        _moveTween?.Stop();
        _rotateTween?.Stop();

        _moveTween = camera.MoveTo(randomTransform.position, 0.35f, EaseType.InOutQuart);
        _rotateTween = camera.RotateTo(randomTransform.rotation, 0.35f, EaseType.InOutQuart);
    }
}
