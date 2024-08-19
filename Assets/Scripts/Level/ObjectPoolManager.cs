using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, CustoObjectmPool<MonoBehaviour>> _objecstPool = new Dictionary<string, CustoObjectmPool<MonoBehaviour>>();
    private WaveManager _waveManager;
    private Dictionary<GameObject, Coroutine> _courotines = new Dictionary<GameObject, Coroutine>();

    public void Initialize(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.OnWaveComplete += OnWaveComplete;
    }

    private void OnDisable()
    {
        _waveManager.OnWaveComplete -= OnWaveComplete;
    }
    
    private void OnWaveComplete()
    {
        foreach (var key in _courotines.Keys)
            StopCoroutine(_courotines[key]);
        _courotines.Clear();

        ClearPools();
    }

    private void ClearPools()
    {
        foreach (var key in _objecstPool.Keys)
        {
            _objecstPool[key].ReleaseAll();
        }
    }

    public MonoBehaviour GetObject(MonoBehaviour objectPrefab)
    {
        if (!_objecstPool.ContainsKey(objectPrefab.name.ToString()))
            _objecstPool.Add(objectPrefab.name.ToString(), new CustoObjectmPool<MonoBehaviour>(objectPrefab, 0));

        return _objecstPool[objectPrefab.name.ToString()].Get();
    }

    public void ReleaseObject(MonoBehaviour objectPrefab, float interval)
    {
        Coroutine coroutine = StartCoroutine(ReleaseObjectCourotine(objectPrefab, interval));
        if (!_courotines.ContainsKey(objectPrefab.gameObject))
            _courotines.Add(objectPrefab.gameObject, coroutine);
    }

    public void ReleaseObject(MonoBehaviour objectPrefab)
    {
        if (_courotines.ContainsKey(objectPrefab.gameObject))
        {
            StopCoroutine(_courotines[objectPrefab.gameObject]);
            _courotines.Remove(objectPrefab.gameObject);
        }
        _objecstPool[objectPrefab.name.ToString()].Release(objectPrefab);
    }

    private IEnumerator ReleaseObjectCourotine(MonoBehaviour objectPrefab, float interval)
    {
        yield return new WaitForSeconds(interval);
        if (_courotines.ContainsKey(objectPrefab.gameObject))
            _courotines.Remove(objectPrefab.gameObject);
        _objecstPool[objectPrefab.name.ToString()].Release(objectPrefab);
    }
}
