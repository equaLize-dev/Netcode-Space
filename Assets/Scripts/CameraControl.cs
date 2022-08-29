using UnityEngine;

public sealed class CameraControl : MonoBehaviour
{
    [SerializeField, Min(0)] private float sensitivity;
    [SerializeField, Min(0)] private float bodyFollowingIntensity;
    [SerializeField, Range(-90.0f, 0.0f)] private float minVerticalAngle;
    [SerializeField, Range(0.0f, 90.0f)] private float maxVerticalAngle;

    private Quaternion _rotation;
    private Transform _parent;

    public Transform Parent { set => _parent = value; }
    
    private bool KeyboardInput => 
        Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.position = 
            Vector3.Lerp(transform.position, _parent.position, Time.deltaTime * bodyFollowingIntensity);
        
        if (!KeyboardInput)
        {
            _rotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            _rotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            _rotation.x = Mathf.Clamp(_rotation.x, minVerticalAngle, maxVerticalAngle);
            transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        }

        else
        {
            if (_parent != null)
            {
                _rotation.x += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                _rotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
            }
        }
    }
}