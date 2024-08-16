using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, CustoObjectmPool<Bullet>> _bulletsPools = new Dictionary<string, CustoObjectmPool<Bullet>>();
    private Dictionary<string, CustoObjectmPool<ExplosionEffect>> _effectsPool= new Dictionary<string, CustoObjectmPool<ExplosionEffect>>();
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
        foreach (var key in _bulletsPools.Keys)
        {
            _bulletsPools[key].ReleaseAll();
        }

        foreach (var key in _effectsPool.Keys)
        {
            _effectsPool[key].ReleaseAll();
        }
    }

    public Bullet GetBullet(Bullet bulletPrefab)
    {
        if (!_bulletsPools.ContainsKey(bulletPrefab.name.ToString()))
            _bulletsPools.Add(bulletPrefab.name.ToString(), new CustoObjectmPool<Bullet>(bulletPrefab, 0));

        return _bulletsPools[bulletPrefab.name.ToString()].Get();
    }

    public ExplosionEffect GetEffect(ExplosionEffect effectPrefab)
    {
        if (!_effectsPool.ContainsKey(effectPrefab.name.ToString()))
            _effectsPool.Add(effectPrefab.name.ToString(), new CustoObjectmPool<ExplosionEffect>(effectPrefab, 0));

        return _effectsPool[effectPrefab.name.ToString()].Get();
    }

    public void ReleaseBullet(Bullet bulletPrefab, float interval)
    {
        Coroutine coroutine = StartCoroutine(ReleaseBulletCourotine(bulletPrefab, interval));
        if (!_courotines.ContainsKey(bulletPrefab.gameObject))
            _courotines.Add(bulletPrefab.gameObject, coroutine);
    }

    public void ReleaseEffect(ExplosionEffect effectPrefab, float interval)
    {
        Coroutine coroutine = StartCoroutine(ReleaseEffectCourotine(effectPrefab, interval));
        if (!_courotines.ContainsKey(effectPrefab.gameObject))
            _courotines.Add(effectPrefab.gameObject, coroutine);
    }

    public void ReleaseBullet(Bullet bulletPrefab)
    {
        if (_courotines.ContainsKey(bulletPrefab.gameObject))
        {
            StopCoroutine(_courotines[bulletPrefab.gameObject]);
            _courotines.Remove(bulletPrefab.gameObject);
        }
        _bulletsPools[bulletPrefab.name.ToString()].Release(bulletPrefab);
    }

    public void ReleaseEffect(ExplosionEffect effectPrefab)
    {
        if (_courotines.ContainsKey(effectPrefab.gameObject))
        {
            StopCoroutine(_courotines[effectPrefab.gameObject]);
            _courotines.Remove(effectPrefab.gameObject);
        }
        _effectsPool[effectPrefab.name.ToString()].Release(effectPrefab);
    }

    private IEnumerator ReleaseBulletCourotine(Bullet bulletPrefab, float interval)
    {
        yield return new WaitForSeconds(interval);
        if (_courotines.ContainsKey(bulletPrefab.gameObject))
            _courotines.Remove(bulletPrefab.gameObject);
        _bulletsPools[bulletPrefab.name.ToString()].Release(bulletPrefab);
    }

    private IEnumerator ReleaseEffectCourotine(ExplosionEffect effectPrefab, float interval)
    {
        yield return new WaitForSeconds(interval);
        if (_courotines.ContainsKey(effectPrefab.gameObject))
            _courotines.Remove(effectPrefab.gameObject);
        _effectsPool[effectPrefab.name.ToString()].Release(effectPrefab);
    }
}
