using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Yautl;

public class MenuCameraPathing : MonoBehaviour
{
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private float speed = 1;
    [SerializeField] private EaseType easeType = EaseType.InOutSine;
    [SerializeField] private bool startAtFirst = false;

    public void Start()
    {
        if (startAtFirst) transform.position = pathPoints[0].position;
    }

    public void MoveToPosition(int transIndex, BaseMenu starterMenu, BaseMenu endMenu)
    {
        starterMenu.HideMenu();
        transIndex = Mathf.Clamp(transIndex, 0, pathPoints.Count - 1);
        transform.MoveTo(pathPoints[transIndex].position, speed, easeType).OnComplete(() => endMenu.ShowMenu());
    }
}
