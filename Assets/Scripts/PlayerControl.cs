using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new();
    [SerializeField] private NetworkVariable<Vector3> networkPositionDirection = new();
    //[SerializeField] private NetworkVariable<Vector3> networkRotationDirection = new();
    [SerializeField] private NetworkVariable<float> forwardBackPosition = new();
    [SerializeField] private NetworkVariable<float> leftRightPosition = new();

    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _direction;
    
    // client caching
    private Vector3 _oldInputPosition;
    //private Vector3 _oldInputRotation;

    private static readonly int s_VerticalMove = Animator.StringToHash("VerticalMove");
    private static readonly int s_Idle = Animator.StringToHash("Idle");

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }
        
        ClientMoveAndRotate();
        ClientVisuals();
    }

    private void ClientInput()
    {
        // player position and rotation input
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        if (_oldInputPosition != _direction)
        {
            _oldInputPosition = _direction;
            UpdateClientPositionServerRpc(_direction * speed);
        }
        
        // player state changes based on input
        if (_direction.x != 0 || _direction.z != 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Move);
        }
        
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
    }

    private void ClientMoveAndRotate()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            _controller.Move(networkPositionDirection.Value * Time.deltaTime);
        }
    }    
    
    private void ClientVisuals()
    {
        if (networkPlayerState.Value == PlayerState.Move)
        {
            _animator.SetFloat(s_VerticalMove, 1);
        }
        
        else if (networkPlayerState.Value == PlayerState.Idle)
        {
            _animator.SetFloat(s_VerticalMove, 0);
        }
    }

    [ServerRpc]
    private void UpdateClientPositionServerRpc(Vector3 newPositionDirection)
    {
        networkPositionDirection.Value = newPositionDirection;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        networkPlayerState.Value = newState;
    }
}

public enum PlayerState
{ 
    Idle, 
    Move
}
