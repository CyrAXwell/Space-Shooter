using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, CustoObjectmPool<Bullet>> _bulletsPools = new Dictionary<string, CustoObjectmPool<Bullet>>();
    private Dictionary<string, CustoObjectmPool<ExplosionEffect>> _effectsPool= new Dictionary<string, CustoObjectmPool<ExplosionEffect>>();
    private WaveManager _waveManager;

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
        ClearPools();
    }

    private void ClearPools()
    {
        
        foreach (var key in _bulletsPools.Keys)
        {
            _bulletsPools[key].ReleaseAll();
        }
    }

    public Bullet GetBullet(Bullet bulletPrefab)
    {
        if (!_bulletsPools.ContainsKey(bulletPrefab.name.ToString()))
            _bulletsPools.Add(bulletPrefab.name.ToString(), new CustoObjectmPool<Bullet>(bulletPrefab, 0));

        return _bulletsPools[bulletPrefab.name.ToString()].Get();
    }

    public void ReleaseBullet(Bullet bulletPrefab, float interval)
    {
        StartCoroutine(ReleaseBulletCourotine(bulletPrefab, interval));
    }

    public void ReleaseBullet(Bullet bulletPrefab)
    {
        _bulletsPools[bulletPrefab.name.ToString()].Release(bulletPrefab);
    }

    private IEnumerator ReleaseBulletCourotine(Bullet bulletPrefab, float interval)
    {
        yield return new WaitForSeconds(interval);
        _bulletsPools[bulletPrefab.name.ToString()].Release(bulletPrefab);
    }
}
