using Unity.Netcode;
using UnityEngine;

public class CameraControl : NetworkBehaviour
{
    [SerializeField, Min(0)] private float sensitivity;
    [SerializeField, Min(0)] private float bodyFollowingIntensity;
    [SerializeField, Min(0)] private float bodyRotationIntensity;
    [SerializeField, Range(-90.0f, 0.0f)] private float minVerticalAngle;
    [SerializeField, Range(0.0f, 90.0f)] private float maxVerticalAngle;

    private Quaternion _idleRotation;
    private Transform _parent;

    public Transform Parent { set => _parent = value; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            transform.SetParent(null, worldPositionStays:true);

            _idleRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            _idleRotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            _idleRotation.x = Mathf.Clamp(_idleRotation.x, minVerticalAngle, maxVerticalAngle);
            transform.rotation = Quaternion.Euler(_idleRotation.x, _idleRotation.y, 0);
        }

        else
        {
            if (_parent != null)
            {
                transform.SetParent(_parent, worldPositionStays:true);
                transform.position = 
                    Vector3.Lerp(transform.position, _parent.position, Time.deltaTime * bodyFollowingIntensity);
                
                _idleRotation.x += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                _idleRotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                transform.rotation = Quaternion.Euler(_idleRotation.x, _idleRotation.y, 0);
                _parent.rotation = 
                    Quaternion.Slerp(_parent.rotation, transform.rotation, Time.deltaTime * bodyRotationIntensity);
            }
        }
    }
}