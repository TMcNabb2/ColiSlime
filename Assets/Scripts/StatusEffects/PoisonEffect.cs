using System;
using UnityEngine;

public class PoisonEffect : StatusEffectManager.StatusEffect, IStaticSettable
{
    public float damagePerSecond;
    public float poisonDuration;

    public static new PoisonEffect prefab;

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
        prefab = (PoisonEffect)set;
    }

    public override void TakeEffect(IEffectStructable effectStructable)
    {
        base.TakeEffect(effectStructable);
        Tuple<float, float> effectParams = (Tuple<float, float>)effectStructable.GetDataFromStruct();
        poisonDuration = effectParams.Item1;
        damagePerSecond = effectParams.Item2;

        statusName = "Poison";
        statusDuration = poisonDuration;
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
        // Example: Instantiate poison particles
        GameObject poisonParticles = new GameObject("PoisonParticles");
        poisonParticles.transform.SetParent(transform);
        poisonParticles.transform.localPosition = Vector3.zero;
        // Add particle system component and configure it for poison effect
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
                Debug.Log($"Poison damage: {damagePerSecond} to {effectedObject.name}");
            }
            
            timeSinceLastDamage = 0f;
        }
    }
} 