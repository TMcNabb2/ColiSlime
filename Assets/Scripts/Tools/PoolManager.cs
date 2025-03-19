using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
	public Dictionary<string, PrefabPool> Pools = new();

	[SerializeField] private SerializedPrefabPool[] _pools;

	protected override void Initialize()
	{
		for (int i = 0; i < _pools.Length; i++)
		{
			if (_pools[i] == null && !_pools[i].Initialized)
			{
				_pools[i].Create();
				Pools.Add(_pools[i].Prefab.name, _pools[i].Pool);
			}
		}
	}
}
