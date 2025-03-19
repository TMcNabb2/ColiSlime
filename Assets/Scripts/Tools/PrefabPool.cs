using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class PrefabPool : ObjectPool<GameObject>
{
	public string Name { get; }
	public GameObject Prefab { get; }
	public Transform Parent { get; }

	public PrefabPool(GameObject prefab, Transform parent = null) : base(
		createFunc: () => Object.Instantiate(prefab, parent),
		actionOnGet: o => o.SetActive(true),
		actionOnRelease: o => o.SetActive(false),
		actionOnDestroy: Object.Destroy)
	{
		Name = prefab.name;
		Prefab = prefab;
		Parent = parent;
	}
}

[Serializable]
public class SerializedPrefabPool
{
	[SerializeField, Prefab] private GameObject _prefab;
	[SerializeField] private Transform _parent;

	private PrefabPool _pool = null;
	public bool Initialized => _pool != null;
	public PrefabPool Pool => _pool;

	public GameObject Prefab => _prefab;

	public Transform Parent => _parent;

	public void Create()
	{
		if (!Initialized && Prefab != null)
		{
			_pool = new(Prefab, Parent);
			Debug.Assert(Initialized);
		}
	}
}
