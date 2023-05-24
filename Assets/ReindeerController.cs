using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReindeerController : MonoBehaviour
{
    public UnityEvent<bool> OnFinished = new(); //true if complete, false if not.

    [SerializeField] private TrashSpawner _trashSpanwer;
    private ReindeerRandomSpawner _reindeerSpawner = new();
    
    private List<GameObject> _reindeerHolder = new();
    private int _retrieveCount;
    private int _lostCount;
    private int _needCount;
    private int _createCount;

    private float _deerLastRowZ = 1.25f;
    private float _deerFrontbackSpacing = 0.75f;
    private float _deerSideSpacing = 0.2f;
    private float _deerHeight = 0.0f;

    public int ReindeerCount => _retrieveCount;
    public int NeedCount => _needCount;
    public int LostCount => _lostCount;

    public void ResetAndCreateRaindeers(GameObject prefab, int createCount, int needCount)
    {
        ResetReindeer(createCount, needCount);

        for (int i = 0; i < _createCount; i++)
        {
            Vector3 pos = transform.position;
            pos.z = _deerLastRowZ + i / 2 * _deerFrontbackSpacing;
            pos.y = _deerHeight;
            pos.x = i % 2 == 0 ? _deerSideSpacing : -_deerSideSpacing;

            var reindeerPrefab = Instantiate(prefab, transform);
            reindeerPrefab.transform.localPosition = pos;
            reindeerPrefab.transform.rotation = transform.rotation;
            _reindeerHolder.Add(reindeerPrefab);

            reindeerPrefab.SetActive(false);
        }

        _reindeerSpawner.SpawnReindeers(prefab, createCount);
        _trashSpanwer.SpawnTrashes(20);
    }

    private void ResetReindeer(int createCount, int needCount)
    {
        foreach (var reindeer in _reindeerHolder)
        {
            Destroy(reindeer.gameObject);
        }
        _reindeerHolder.Clear();
        _retrieveCount = 0;
        _lostCount = 0;
        _createCount = createCount;
        _needCount = Mathf.Max(needCount, createCount);

        _reindeerSpawner.ResetSpawned();
        _trashSpanwer.ResetSpawned();
    }

    public void OnRetrieve()
    {
        _reindeerHolder[_retrieveCount].SetActive(true);
        _retrieveCount++;
        Debug.Log("OnRetrieve. current:" + _retrieveCount);
        if (_retrieveCount == _needCount)
        {
            Debug.LogWarning("Complete!");
            OnFinished.Invoke(true);
        }
    }

    public void OnLost()
    {
        _lostCount++;
        Debug.Log("OnLost. lost: " + _lostCount);
        if (_createCount - _lostCount < _needCount)
        {
            Debug.LogWarning("GameOver...");
            OnFinished.Invoke(false);
        }
    }
}
