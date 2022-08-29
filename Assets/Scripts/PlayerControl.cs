using Unity.Netcode;
using UnityEngine;

public sealed class PlayerControl : NetworkBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float animationInterpolateMultiplier = 7f;
    [SerializeField, Min(0)] private float cameraFollowingIntensity = 5f;
    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new();
    [SerializeField] private NetworkVariable<Vector3> networkPlayerPosition = new();
    [SerializeField] private NetworkVariable<Quaternion> networkPlayerRotation = new();

    private CharacterController _controller;
    private Transform _camera;
    private Animator _animator;
    private Vector3 _direction;
    
    private Vector3 _oldInputPosition;
    private Quaternion _oldInputRotation;

    private static readonly int s_VerticalMove = Animator.StringToHash("VerticalMove");
    private static readonly int s_HorizontalMove = Animator.StringToHash("HorizontalMove");

    public Vector3 NetworkPlayerPosition
    {
        set => networkPlayerPosition.Value = value;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
            var cam = FindObjectOfType<CameraControl>();
            cam.Parent = transform;
            cam.enabled = true;
            _camera = cam.gameObject.transform;
        }
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }
        
        ClientMove();
        ClientVisuals();
    }

    private void ClientInput()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        if (_oldInputPosition != _direction)
        {
            _oldInputPosition = _direction;
            UpdateClientPositionServerRpc(_direction);
        }

        if (_oldInputRotation != _camera.rotation)
        {
            _oldInputRotation = _camera.rotation;
            UpdateClientRotationServerRpc(_camera.rotation);
        }

        UpdatePlayerStateServerRpc(PlayerState.Move);
    }

    private void ClientMove()
    {
        if (networkPlayerPosition.Value != Vector3.zero)
        {
            _controller.Move(
                transform.TransformDirection(networkPlayerPosition.Value )* (Time.deltaTime * speed));
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation, networkPlayerRotation.Value, Time.deltaTime * cameraFollowingIntensity);
        }
    }    
    
    private void ClientVisuals()
    {
        if (networkPlayerState.Value == PlayerState.Move)
        {
            if (IsClient && IsOwner)
            {
                _animator.SetFloat(s_HorizontalMove,
                    Mathf.Lerp(_animator.GetFloat(s_HorizontalMove), _direction.x,
                        Time.deltaTime * animationInterpolateMultiplier));
                
                _animator.SetFloat(s_VerticalMove,
                    Mathf.Lerp(_animator.GetFloat(s_VerticalMove), _direction.z,
                        Time.deltaTime * animationInterpolateMultiplier));
            }

            else
            {
                _animator.SetFloat(s_HorizontalMove,
                    Mathf.Lerp(_animator.GetFloat(s_HorizontalMove), networkPlayerPosition.Value.x,
                        Time.deltaTime * animationInterpolateMultiplier));
                
                _animator.SetFloat(s_VerticalMove,
                    Mathf.Lerp(_animator.GetFloat(s_VerticalMove), networkPlayerPosition.Value.z,
                        Time.deltaTime * animationInterpolateMultiplier));
            }
        }
    }

    [ServerRpc]
    private void UpdateClientPositionServerRpc(Vector3 newPositionDirection)
    {
        networkPlayerPosition.Value = newPositionDirection;
    }    
    
    [ServerRpc]
    private void UpdateClientRotationServerRpc(Quaternion newRotation)
    {
        networkPlayerRotation.Value = newRotation;
    }

    [ServerRpc]
    private void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        networkPlayerState.Value = newState;
    }
}

public enum PlayerState
{ 
    Idle, 
    Move
}