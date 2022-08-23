using UnityEngine;

public sealed class CameraLobbyRotation : MonoBehaviour
{
    private Vector3 _startupPosition;
    private Quaternion _startupRotation;

    public void Disable()
    {
        transform.SetPositionAndRotation(_startupPosition, _startupRotation);
        enabled = false;
    }

    private void Start()
    {
        _startupPosition = transform.position;
        _startupRotation = transform.rotation;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(1, 1, 1), Time.deltaTime);
    }
}
