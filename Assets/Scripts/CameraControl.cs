using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField, Min(0)] private float sensitivity;
    [SerializeField, Min(0)] private float slerpIntensity;
    [SerializeField, Range(-90.0f, 0.0f)] private float minVerticalAngle;
    [SerializeField, Range(0.0f, 90.0f)] private float maxVerticalAngle;

    private Quaternion _startRotation;
    private Quaternion _idleRotation;
    private Quaternion _activeRotation;
    private Transform _parent;

    public Transform Parent { set => _parent = value; }

    private void Start()
    {
        _startRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0)
        {
            IdleRotation();
        }
        
        else
        {
            if (_parent != null)
            {
                transform.SetParent(_parent);
            }
            
            ActiveRotation();
        }
    }

    private void IdleRotation()
    {
        _activeRotation = Quaternion.identity;
        
        _idleRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        _idleRotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        
        _idleRotation.x = Mathf.Clamp(_idleRotation.x, minVerticalAngle, maxVerticalAngle);
        transform.rotation = Quaternion.Euler(_idleRotation.x, _idleRotation.y, 0);
        
    }

    private void ActiveRotation()
    {
        _idleRotation = Quaternion.identity;
        transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, Time.deltaTime * slerpIntensity);
    }
}
