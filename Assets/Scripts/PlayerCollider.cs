using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour
{
    [SerializeField] private int respawnDelay;
    private readonly NetworkVariable<int> score = new();
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (IsClient && IsOwner) //TODO: Replace without Update()
        {
            _uiManager.UpdatePlayerScore(score.Value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SolidObject"))
        {
            var tokenSource = new CancellationTokenSource();
            Task respawn = RespawnAsync(respawnDelay, tokenSource.Token);
            Task.Run(() => respawn, tokenSource.Token);
            gameObject.SetActive(false);
        }

        else if (other.CompareTag("Crystal"))
        {
            if (IsServer)
            {
                score.Value += 1;
                other.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    private async Task RespawnAsync(int delay, CancellationToken token)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        await Task.Delay(delay * 1000, token);
        gameObject.transform.position = Vector3.zero;
        gameObject.SetActive(true);
        linkedTokenSource.Dispose();
    }
}