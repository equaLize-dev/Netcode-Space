using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public sealed class SolidObject : MonoBehaviour
{
    [SerializeField] private int respawnDelay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (other.TryGetComponent<PlayerControl>(out var player))
            {
                var tokenSource = new CancellationTokenSource();
                Task respawn = RespawnPlayerAsync(player, respawnDelay, tokenSource.Token);
                Task.Run(() => respawn, tokenSource.Token);
            }
        }
    }
    
    private async Task RespawnPlayerAsync(PlayerControl player, int delay, CancellationToken token)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        player.NetworkPlayerPosition = Vector3.zero;
        NetworkObject networkPlayer = player.GetComponent<NetworkObject>();
        NetworkObject respawnedPlayer = Instantiate(networkPlayer);
        ulong clientId = player.OwnerClientId;
        respawnedPlayer.transform.position = Vector3.zero;
        respawnedPlayer.gameObject.SetActive(false);
        networkPlayer.Despawn();
        await Task.Delay(delay * 1000, token);
        respawnedPlayer.gameObject.SetActive(true);
        respawnedPlayer.SpawnWithOwnership(clientId);
        linkedTokenSource.Dispose();
    }
}
