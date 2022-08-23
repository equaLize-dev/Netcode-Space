using System;
using Unity.Netcode;
using UnityEngine;

public sealed class PlayerScore : NetworkBehaviour, IComparable<int>
{
    public NetworkVariable<int> Score { get; } = new();

    private void Awake()
    {
        Score.OnValueChanged += (_, newValue) =>
        {
            if (IsClient && IsOwner)
            {
                FindObjectOfType<UIManager>().UpdatePlayerScore(newValue);
            }
        };
    }

    public int CompareTo(int other)
    {
        return Score.Value > other ? 1 : Score.Value < other ? -1 : 0;
    }
    
}