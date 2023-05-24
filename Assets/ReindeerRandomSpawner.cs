using System.Collections.Generic;
using UnityEngine;

public class ReindeerRandomSpawner
{
    private Vector3 _mapSize;
    private Vector3 _mapPos;
    private float _spawnOffset = 1.0f;
    private float _spawnHeightBase = 3.0f;
    private List<GameObject> _spawned = new();

    public void SpawnReindeers(GameObject reindeerPrefab, int createCount)
    {
        _mapPos = GameManager.Instance.GameAreaLocation;

        _mapSize = GameManager.Instance.GameAreaSize;
        _mapSize.x /= 2;
        _mapSize.x -= _spawnOffset;
        _mapSize.z /= 2;
        _mapSize.z -= _spawnOffset;

        for (int i = 0; i < createCount; i++)
        {
            var x = Random.Range(-_mapSize.x, _mapSize.x);
            var y = Random.Range(_spawnHeightBase - _spawnOffset, _spawnHeightBase + _spawnOffset);
            var z = Random.Range(-_mapSize.z, _mapSize.z);

            SpawnReindeer(reindeerPrefab, _mapPos + new Vector3(x,y,z));
        }
    }

    private void SpawnReindeer(GameObject reindeerPrefab, Vector3 createLocation)
    {
        GameObject go = new GameObject();
        var reindeer = go.AddComponent<ReinDeer>();
        reindeer.transform.position = createLocation;
        reindeer.transform.rotation = Random.rotation;
        reindeer.Initialize(reindeerPrefab);
        _spawned.Add(go);
    }

    public void ResetSpawned()
    {
        int count = 0;
        foreach(var go in _spawned)
        {
            if( go == null)
            {
                continue;
            }
            count++;
            GameObject.Destroy(go);
        }
        //Debug.LogWarning($"Reseting spawned end: {count}/{_spawned.Count}");
        _spawned.Clear();
    }
}
