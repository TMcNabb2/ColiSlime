using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager: MonoBehaviour
{
    private static List<StatusEffect> EffectedObjects = new List<StatusEffect>();
    public EffectReferences EffectReferences;
    
    public static void ApplyEffect(StatusEffect effect, IEffectStructable effectParams, IEffectable affectedObject, float? duration)
    {
        StatusEffect toInstantiate = Instantiate(effect.gameObject,affectedObject.transform).GetComponent<StatusEffect>();
        
        toInstantiate.statusDuration = duration != null ? duration.Value : effect.statusDuration;
        
        toInstantiate.TakeEffect(effectParams);
    }

    public static bool TryGetObjectEffectable(GameObject gameObject, out IEffectable effectable)
    {
        effectable = gameObject.GetComponent<IEffectable>();
        if (effectable != null)
        {
            return true;
        }

        return false;
    }

    public static List<StatusEffect> GetEffectedObjects()
    {
        return EffectedObjects;
    }

    public static bool TryGetEffectFromEffectedObjet(GameObject gameObject, out StatusEffect effect)
    {
        
        if (gameObject.TryGetComponent<StatusEffect>(out effect))
        {
            return true;
        }

        return false;
    }

    private static void AddEffectToList(StatusEffect effect)
    {
        EffectedObjects.Add(effect);
    }

    private static void RemoveEffectFromList(StatusEffect effect)
    {
        EffectedObjects.Remove(effect);
    }


    private void Awake()
    {
        foreach (object effect in EffectReferences.effects)
        {

            if (effect is IStaticSettable staticEffect) 
            {
                staticEffect.Set(effect);
            }
        }
    }

    public abstract class StatusEffect : MonoBehaviour
    {
        public string statusName;
        public float statusDuration;

        public GameObject effectedObject;

        public static object prefab;


        [HideInInspector]
        public struct EffectParameters : IEffectStructable
        {
            public object GetDataFromStruct()
            {
                return null;
            }
        }


        protected float _currentTime;

        protected virtual void OnTick()
        {

        }

        protected abstract void OnFirst();


        public virtual void TakeEffect(IEffectStructable effectParams)
        {
            _currentTime = statusDuration;
            StatusEffectManager.AddEffectToList(this);
        }


        private void Start()
        {
            OnFirst();
        }

        private void Update()
        {
            if (_currentTime >= 0)
            {
                OnTick();
            }
            else
            {
                Destroy(this.gameObject);
            }
            _currentTime -= Time.deltaTime;
        }

        void OnDestroy()
        {
            StatusEffectManager.RemoveEffectFromList(this);
        }
    }
}


public interface IEffectable
{
    public Transform transform { get; set; }
}


public interface IEffectStructable
{
    public abstract object GetDataFromStruct();
}

public interface IStaticSettable
{
    public abstract void Set(object set);
}