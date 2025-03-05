using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private static List<StatusEffect> EffectedObjects = new List<StatusEffect>();
    
    
    public static void ApplyEffect(StatusEffect effect, IEffectable affectedObject, float? duration)
    {
        StatusEffect toInstantiate = Instantiate(effect.gameObject).GetComponent<StatusEffect>();
        
        toInstantiate.statusDuration = duration != null ? duration.Value : effect.statusDuration;
        
        affectedObject.TakeEffect(toInstantiate.statusDuration,toInstantiate);
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

    public static void AddEffectToList(StatusEffect effect)
    {
        EffectedObjects.Add(effect);
    }

    public static void RemoveEffectFromList(StatusEffect effect)
    {
        EffectedObjects.Remove(effect);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public interface IEffectable
{
    public void TakeEffect(float duration,StatusEffect effect);
}
