using Unity.Netcode;
using UnityEngine;

public sealed class Crystal : NetworkBehaviour
{
    [SerializeField] private int reward;
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsClient && IsOwner)
        {
            if (other.TryGetComponent<PlayerScore>(out var player))
            {
                player.Score.Value += reward;
                CrystalSpawner.Instance.CurrentCrystalsCount.Value--;
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
