using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public sealed class EndgameManager : MonoBehaviour
{
    [SerializeField] private Leaderboard leaderboard;
    private Action OnUpdate;
    
    private void Start()
    {
        CrystalSpawner.Instance.CurrentCrystalsCount.OnValueChanged += (_, newValue) =>
        {
            if (newValue == 0)
            {
                if (CrystalSpawner.Instance.IsAllCrystalsSpawned.Value)
                {
                    // Invocation in the next Update() because new Score has to be updated on the client.
                    OnUpdate += () =>
                    {
                        PlayerScore[] winners = FindObjectsOfType<PlayerScore>()
                            .OrderByDescending(player => player.Score.Value).ToArray();
                        leaderboard.ShowLeaderboard(winners);

                        foreach (var player in winners)
                        {
                            player.GetComponent<PlayerControl>().enabled = false;
                        }

                        OnUpdate = null;
                    };
                }
            }
        };
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }
}
