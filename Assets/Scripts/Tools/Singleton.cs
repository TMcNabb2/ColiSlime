using UnityEngine;

/// <summary>
/// Replace the inhered <see cref="MonoBehaviour"/> with <see cref="Singleton{T}"/>.
/// <br />
/// Use this if you need global data. Use <seealso cref="PersistentSingleton{T}"/> if you need the script to be available between scenes.
/// </summary>
/// <typeparam name="T">T refers to the class that you desire to inherent <see cref="Singleton{T}"/>.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	[field: SerializeField] public bool AutoUnparentOnAwake { get; protected set; } = true;

#pragma warning disable UDR0001 // Domain Reload Analyzer
	protected static T instance;
#pragma warning restore UDR0001 // Domain Reload Analyzer
	public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;

    /// <summary>
    /// Use this to access non-static methods and properties.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)   
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name + " Auto-Generated");
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// Make sure to call base.Awake() in override if you need awake.
    /// </summary>
    protected virtual void Awake()
    {
        if (!Application.isPlaying) return;

        if (AutoUnparentOnAwake)
        {
            transform.SetParent(null);
        }

        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
			return;
		}
		Initialize();
	}

    /// <summary>
    /// Use this method for initialization on <see cref="Awake"/>.
    /// </summary>
    protected abstract void Initialize();
}
