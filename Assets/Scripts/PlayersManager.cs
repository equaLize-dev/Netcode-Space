using Unity.Netcode;
using UnityEngine;

public sealed class PlayersManager : NetworkSingleton<PlayersManager>
{
    private NetworkVariable<int> _playersInGame = new();

    public int PlayersInGame => _playersInGame.Value;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += id =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"Player {id} just connected...");
                _playersInGame.Value++;
            }
        };       
        
        NetworkManager.Singleton.OnClientDisconnectCallback += id =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"Player {id} just disconnected...");
                _playersInGame.Value--;
            }
        };
    }
}
