using System;
using UnityEngine;

public class BurningEffect : StatusEffectManager.StatusEffect, IStaticSettable
{
    public float damagePerSecond;
    public float burnDuration;

    public static new BurningEffect prefab;

    public static void ApplyEffect(GameObject targetGameobject, float duration, float damagePerSecond)
    {
        if (targetGameobject == null)
        {
            return;
        }

        IEffectable effectable;

        if (StatusEffectManager.TryGetObjectEffectable(targetGameobject, out effectable))
        {
            EffectParameters currentParameters = new EffectParameters(duration, damagePerSecond);

            StatusEffectManager.ApplyEffect(prefab, currentParameters, effectable, duration);
        }
    }

    public void Set(object set)
    {
        prefab = (BurningEffect)set;
    }

    public override void TakeEffect(IEffectStructable effectStructable)
    {
        base.TakeEffect(effectStructable);
        Tuple<float, float> effectParams = (Tuple<float, float>)effectStructable.GetDataFromStruct();
        burnDuration = effectParams.Item1;
        damagePerSecond = effectParams.Item2;

        statusName = "Burning";
        statusDuration = burnDuration;
    }

    public new struct EffectParameters : IEffectStructable
    {
        float duration;
        float damage;

        public EffectParameters(float duration, float damage)
        {
            this.duration = duration;
            this.damage = damage;
        }

        public object GetDataFromStruct()
        {
            return new Tuple<float, float>(this.duration, this.damage);
        }
    }

    private float timeSinceLastDamage = 0f;
    
    protected override void OnFirst()
    {
        // Apply visual effect if needed
        // Example: Instantiate fire particles
        GameObject fireParticles = new GameObject("FireParticles");
        fireParticles.transform.SetParent(transform);
        fireParticles.transform.localPosition = Vector3.zero;
        // Add particle system component and configure it for fire effect
    }

    protected override void OnTick()
    {
        timeSinceLastDamage += Time.deltaTime;
        
        // Apply damage every second
        if (timeSinceLastDamage >= 1.0f)
        {
            // Apply damage to the target
            if (effectedObject != null && effectedObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Deal damage
                // This would need to be implemented in the Enemy class
                Debug.Log($"Burning damage: {damagePerSecond} to {effectedObject.name}");
            }
            
            timeSinceLastDamage = 0f;
        }
    }
} 