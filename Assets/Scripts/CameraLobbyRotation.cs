using System;
using UnityEngine;

public class CameraLobbyRotation : MonoBehaviour
{
    private Vector3 _startupPosition;
    private Quaternion _startupRotation;

    public void Disable()
    {
        transform.position = _startupPosition;
        transform.rotation = _startupRotation;
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
