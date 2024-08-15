using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustoObjectmPool<T> where T : MonoBehaviour
{
	private T _prefab;
	private List<T> _objects;
	
	public CustoObjectmPool(T prefab, int prewarmObjects)
	{
		_prefab = prefab;
        _objects = new List<T>();

        for (int i = 0; i < prewarmObjects; i++)
        {
            T obj = Create();
            Release(obj);
        }
	}
	
	public T Get()
	{
        T obj = _objects.FirstOrDefault(x => !x.isActiveAndEnabled);

        if (obj == null)
            obj = Create();

        obj.gameObject.SetActive(true);
        return obj;
	} 
	
	public T Create()
	{
		T obj = GameObject.Instantiate(_prefab);
        _objects.Add(obj);
        return obj;
	}  

	public void Release(T obj)
	{
		obj.gameObject.SetActive(false);
	}

    public void ReleaseAll()
    {
        foreach(T obj in _objects)
            Release(obj);
    }
}
