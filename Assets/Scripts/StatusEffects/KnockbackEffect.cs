using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackEffect : StatusEffectManager.StatusEffect, IStaticSettable
{
    public float addForce;
    public Vector3 forceDirection;

    public static new KnockbackEffect prefab;

    public static void ApplyEffect(GameObject targetGameobject, float addForce, Vector3 forceDirection)
    {
        if (targetGameobject == null)
        {
            return;
        }

        IEffectable effectable;

        if (StatusEffectManager.TryGetObjectEffectable(targetGameobject, out effectable))
        {
            EffectParameters currentParameters = new EffectParameters(addForce, forceDirection);

            StatusEffectManager.ApplyEffect(prefab, currentParameters, effectable, null);
        }
    }

    public void Set(object set)
    {
        prefab = (KnockbackEffect)set;
    }

    /// <summary>
    /// Think of this as the constructor method
    /// </summary>
    /// <param name="effectStructable"></param>
    public override void TakeEffect(IEffectStructable effectStructable)
    {
        base.TakeEffect(effectStructable);
        Tuple<float, Vector3> effectParams = (Tuple<float, Vector3>)effectStructable.GetDataFromStruct();
        addForce = effectParams.Item1;
        forceDirection = effectParams.Item2;

        statusName = "Knockback";
        statusDuration = 0.1f;
    }

    public new struct EffectParameters : IEffectStructable
    {
        float addForce;
        Vector3 forceDirection;


        public EffectParameters(float addForce, Vector3 forceDirection)
        {
            this.addForce = addForce;
            this.forceDirection = forceDirection;
        }


        public object GetDataFromStruct()
        {
            return new Tuple<float,Vector3>(this.addForce,this.forceDirection);
        }
    }

    Rigidbody effectedRB;

    protected override void OnFirst()
    {
        if (effectedObject.TryGetComponent<Rigidbody>(out effectedRB))
        {
            effectedRB.AddForce(forceDirection * addForce, ForceMode.Impulse);
        }
    }

}
