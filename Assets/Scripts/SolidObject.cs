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
            if (other.TryGetComponent<NetworkObject>(out var player))
            {
                var tokenSource = new CancellationTokenSource();
                Task respawn = RespawnPlayerAsync(player, respawnDelay, tokenSource.Token);
                Task.Run(() => respawn, tokenSource.Token);
            }
        }
    }
    
    private async Task RespawnPlayerAsync(NetworkObject player, int delay, CancellationToken token)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        NetworkObject respawnedPlayer = Instantiate(player);
        ulong clientId = player.OwnerClientId;
        respawnedPlayer.transform.position = Vector3.zero;
        respawnedPlayer.gameObject.SetActive(false);
        player.Despawn();
        await Task.Delay(delay * 1000, token);
        respawnedPlayer.gameObject.SetActive(true);
        respawnedPlayer.SpawnWithOwnership(clientId);
        linkedTokenSource.Dispose();
    }
}
