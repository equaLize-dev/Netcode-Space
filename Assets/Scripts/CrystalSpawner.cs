using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO: Refactoring.
public class CrystalSpawner : NetworkSingleton<CrystalSpawner>
{
    [SerializeField] private NetworkObject crystalPrefab;
    [SerializeField] private Transform crystalSpawnArea;
    [SerializeField] private int spawnDelay;
    [SerializeField] private int spawnCount;

    [Tooltip("Objects will spawn without changing the Y-axis if enabled.")]
    [SerializeField] private bool spawnOnlyXZ;

    private NetworkVariable<int> _awailableCrystalsCount = new();
    private NetworkVariable<bool> _isAllCrystalsSpawned = new();

    private int _spawns;
    private float _time;
    private bool _readyToSpawn;

    public void Spawn()
    {
        if(!NetworkManager.Singleton.IsServer) return;
        _readyToSpawn = true;
    }

    public void DecreaseSpawnedCrystalsCount(int decrease)
    {
        if(!NetworkManager.Singleton.IsServer) return;
        _awailableCrystalsCount.Value -= decrease;
        if (_awailableCrystalsCount.Value == 0 && _isAllCrystalsSpawned.Value)
        {
            foreach (var player in FindObjectsOfType<PlayerControl>())
            {
                player.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    //TODO: Use async void (Task) instead.
    private void Update()
    {
        if(!NetworkManager.Singleton.IsServer) return;
        if (!_readyToSpawn) return;
        
        if (_time >= spawnDelay)
        {
            if (_spawns < spawnCount)
            {
                NetworkObject crystal = Instantiate(crystalPrefab);
                crystal.transform.position = crystalSpawnArea.position;
                crystal.transform.position += RandomPosition(crystalSpawnArea.localScale);
                crystal.Spawn();
                _awailableCrystalsCount.Value++;
                _spawns++;
                _time = 0;
            }
        }

        else
        {
            _time += Time.deltaTime;
        }

        if (_spawns == spawnCount)
        {
            _readyToSpawn = false;
            _isAllCrystalsSpawned.Value = true;
        }
    }

    private Vector3 RandomPosition(Vector3 areaScale)
    {
        if (!spawnOnlyXZ)
        {
            return new Vector3
            {
                x = Random.Range(-(areaScale.x / 2f), areaScale.x / 2f),
                y = Random.Range(-(areaScale.y / 2f), areaScale.y / 2f),
                z = Random.Range(-(areaScale.z / 2f), areaScale.z / 2f)
            };
        }

        return new Vector3
        {
            x = Random.Range(-(areaScale.x / 2f), areaScale.x / 2f),
            y = 0f,
            z = Random.Range(-(areaScale.z / 2f), areaScale.z / 2f)
        };
    }
}