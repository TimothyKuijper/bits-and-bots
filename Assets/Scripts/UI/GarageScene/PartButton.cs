using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PartButton : WorldButton
{
    [Header("Part Menu")]
    [SerializeField] private Part.PartType partType;

    [Header("Physics")]
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private Vector3 force = new(1,0,0);
    [SerializeField] private Vector3 offset = new(0, 2, 0);
    [SerializeField] private float spring = 10;
    [SerializeField] private float damper = .1f;
    [SerializeField] private float minDistance = 0;
    [SerializeField] private float maxDistance = 2;
    [SerializeField] private float tolerance = .025f;

    [Header("Debug")]
    [SerializeField] private Color hingeColor = Color.blue;
    [SerializeField] private float hingeSize = .2f;

    public Vector3 OffsetPosition => _origin == Vector3.zero ? transform.position : _origin + offset;
    private const string SpringSuffix = "Spring";
    private Vector3 _origin;
    private Rigidbody _rigidBody;


    private void Start()
    {
        _origin = transform.position;
        _rigidBody = GetComponent<Rigidbody>();

        SetupSpring();
        onPressed.AddListener(() => _rigidBody.AddForce(force, forceMode));
    }

    private void SetupSpring()
    {
        var newObject = new GameObject();
        newObject.name = name + SpringSuffix;

        var springObject = newObject.AddComponent<SpringJoint>();
        springObject.transform.position = OffsetPosition;
        springObject.spring = spring;
        springObject.damper = damper;
        springObject.minDistance = minDistance;
        springObject.maxDistance = maxDistance;
        springObject.tolerance = tolerance;
        springObject.connectedBody = _rigidBody;
        
        var springRigidBody = springObject.GetComponent<Rigidbody>();
        springRigidBody.useGravity = false;
        springRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void SetPartMenu(PartsScreen partMenu) => onPressed.AddListener(() => partMenu.OpenPart(partType));

    private void OnDrawGizmos()
    {
        Gizmos.color = hingeColor;
        Gizmos.DrawSphere(OffsetPosition, hingeSize);
    }
}
