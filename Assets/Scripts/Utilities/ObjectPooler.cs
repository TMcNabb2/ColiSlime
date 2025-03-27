using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Singleton instance
    public static ObjectPooler Instance { get; private set; }
    
    // Pool configuration
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    // List of pools to create
    public List<Pool> pools;
    
    // Dictionary to hold the pools
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        // Create all pools
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform); // Parent to this object
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    // Method to spawn an object from the pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist!");
            return null;
        }
        
        // Get an object from the pool
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        
        // If all objects are in use, just reuse the one we just got
        if (objectToSpawn.activeInHierarchy)
        {
            objectToSpawn.SetActive(false);
        }
        
        // Set position and rotation
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        
        // Add the object back to the queue
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        // If the object has an IPooledObject interface, call OnObjectSpawn
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        
        return objectToSpawn;
    }
    
    // Add a new pool at runtime
    public void AddPool(string tag, GameObject prefab, int size)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} already exists!");
            return;
        }
        
        Queue<GameObject> objectPool = new Queue<GameObject>();
        
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            objectPool.Enqueue(obj);
        }
        
        poolDictionary.Add(tag, objectPool);
        pools.Add(new Pool { tag = tag, prefab = prefab, size = size });
    }
}

// Interface for objects that can be pooled
public interface IPooledObject
{
    void OnObjectSpawn();
} 