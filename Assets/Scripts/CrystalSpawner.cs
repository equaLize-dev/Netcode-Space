using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSpawner : Singleton<CrystalSpawner>
{
    [SerializeField] private NetworkObject crystalPrefab;
    [SerializeField] private Transform crystalSpawnArea;
    [SerializeField] private int spawnDelay;
    [SerializeField] private int spawnCount;

    [Tooltip("Objects will spawn without changing the Y-axis if enabled.")]
    [SerializeField] private bool spawnOnlyXZ;

    private int _spawns;
    private float _time;
    private bool _readyToSpawn;

    public void Spawn()
    {
        _readyToSpawn = true;
    }

    private void Update()
    {
        if (_readyToSpawn)
        {
            if (_time >= spawnDelay)
            {
                if (_spawns < spawnCount)
                {
                    NetworkObject crystal = Instantiate(crystalPrefab);
                    crystal.transform.position = crystalSpawnArea.position;
                    crystal.transform.position += RandomPosition(crystalSpawnArea.localScale);
                    crystal.Spawn();
                    _spawns++;
                    _time = 0;
                }

                else
                {
                    _readyToSpawn = false;
                }
            }

            else
            {
                _time += Time.deltaTime;
            }
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