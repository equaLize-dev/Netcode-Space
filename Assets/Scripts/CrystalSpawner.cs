using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class CrystalSpawner : NetworkSingleton<CrystalSpawner>
{
    [SerializeField] private NetworkObject crystalPrefab;
    [SerializeField] private Transform crystalSpawnArea;
    [SerializeField] private int spawnDelay;
    [SerializeField] private int spawnCount;
    
    [Tooltip("Objects will spawn without changing the Y-axis if enabled.")]
    [SerializeField] private bool spawnOnlyXZ;

    public NetworkVariable<int> CurrentCrystalsCount { get; } = new();
    public NetworkVariable<bool> IsAllCrystalsSpawned { get; } = new();

    private Action OnUpdate;
    private int _spawns;
    private float _time;
    
    public void Spawn()
    {
        OnUpdate = delegate
        {
            if (!IsServer) return;

            if (_time >= spawnDelay)
            {
                NetworkObject crystal = Instantiate(crystalPrefab);
                crystal.transform.position = crystalSpawnArea.position + RandomPosition(crystalSpawnArea.localScale);
                crystal.Spawn();
                CurrentCrystalsCount.Value++;
                _spawns++;
                _time = 0;

                if (_spawns == spawnCount)
                {
                    IsAllCrystalsSpawned.Value = true;
                    enabled = false;
                }
            }

            else
            {
                _time += Time.deltaTime;
            }
        };
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }
    

    private Vector3 RandomPosition(Vector3 areaScale)
    {
        return new Vector3
        {
            x = Random.Range(-(areaScale.x / 2f), areaScale.x / 2f),
            y = spawnOnlyXZ ? 0 : Random.Range(-(areaScale.y / 2f), areaScale.y / 2f),
            z = Random.Range(-(areaScale.z / 2f), areaScale.z / 2f)
        };
    }
}