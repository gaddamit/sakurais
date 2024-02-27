using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    [SerializeField]
    Transform _targetTransform;
    [SerializeField]
    private float _cameraFollowSpeed = .2f;
    [SerializeField]
    private float _cameraLookSpeed = 2f;
    [SerializeField]
    private float _cameraPivotSpeed = 2f;

    [SerializeField]
    private float _minPivotAngle = -35;
    [SerializeField]
    private float _maxPivotAngle = 35;
    [SerializeField]
    private Transform _cameraPivot;


    private float _lookAngle = 0;
    private float _pivotAngle = 0;

    private void Awake()
    {
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        _targetTransform = player.transform;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        HandleAllCameraMovement();
    }

    private void HandleAllCameraMovement()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _targetTransform.position, ref _cameraFollowVelocity, _cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera(Vector2 movement)
    {
        _lookAngle = _lookAngle + movement.x * _cameraLookSpeed;
        _pivotAngle = _pivotAngle - (movement.y * _cameraPivotSpeed);
        _pivotAngle = Mathf.Clamp(_pivotAngle, _minPivotAngle, _maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _lookAngle;

        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        
        targetRotation = Quaternion.Euler(rotation);
        _cameraPivot.localRotation = targetRotation;
    }
}
