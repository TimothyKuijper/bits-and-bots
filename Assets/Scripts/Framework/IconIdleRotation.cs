using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconIdleRotation : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float angle = 20f;
    [SerializeField] private Vector3 offset;
    

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f + offset.x, 0f + offset.y, Mathf.Sin(Time.time * speed) * angle + offset.z);
    }
}
