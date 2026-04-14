using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuTransferButton : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private MenuCameraPathing menuCameraPath;
    [SerializeField] private int cameraPoint = 1;

    [Header("Menuing")]
    [SerializeField][Tooltip("If left empty, grabs parent")] private BaseMenu myMenu;
    [SerializeField] private BaseMenu nextMenu;

    private void Start()
    {
        if (myMenu == null) myMenu = GetComponentInParent<BaseMenu>();

        var button = GetComponent<Button>();
        button.onClick.AddListener(() => menuCameraPath.MoveToPosition(cameraPoint, myMenu, nextMenu));
    }
}
