using UnityEngine;

public sealed class CameraControl : MonoBehaviour
{
    [SerializeField, Min(0)] private float sensitivity;
    [SerializeField, Min(0)] private float bodyFollowingIntensity;

    private Quaternion _rotation;
    private Transform _parent;

    public Transform Parent { set => _parent = value; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_parent != null)
        {
            transform.position = 
                Vector3.Lerp(transform.position, _parent.position, Time.deltaTime * bodyFollowingIntensity);
        }
        
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            _rotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        }

        else
        {
            _rotation.x += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        }
        
        _rotation.y += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
    }
}