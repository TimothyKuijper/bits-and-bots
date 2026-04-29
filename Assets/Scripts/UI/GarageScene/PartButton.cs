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
    [SerializeField] private Vector3 axis = new(1, 1, 1);

    [Header("Debug")]
    [SerializeField] private Color hingeColor = Color.blue;
    [SerializeField] private float hingeSize = .2f;

    public Vector3 OffsetPosition => _origin == Vector3.zero ? transform.position : _origin + offset;
    private const string HingeSuffix = "Hinge";
    private Vector3 _origin;
    private Rigidbody _rigidBody;


    private void Start()
    {
        _origin = transform.position;
        _rigidBody = GetComponent<Rigidbody>();

        SetupHinge();
        onPressed.AddListener(() => _rigidBody.AddForce(force, forceMode));
    }

    private void SetupHinge()
    {
        var newObject = new GameObject();
        newObject.name = name + HingeSuffix;

        var hingeObject = newObject.AddComponent<HingeJoint>();
        hingeObject.transform.position = OffsetPosition;
        hingeObject.axis = axis;
        hingeObject.connectedBody = _rigidBody;
        
        var hingeRigidBody = hingeObject.GetComponent<Rigidbody>();
        hingeRigidBody.useGravity = false;
        hingeRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void SetPartMenu(PartsScreen partMenu) => onPressed.AddListener(() => partMenu.OpenPart(partType));

    private void OnDrawGizmos()
    {
        Gizmos.color = hingeColor;
        Gizmos.DrawSphere(OffsetPosition, hingeSize);
    }
}
