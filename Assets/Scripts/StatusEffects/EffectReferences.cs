using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectReferences", menuName = "Scriptable Objects/EffectReferences")]
public class EffectReferences : ScriptableObject
{
    public List<StatusEffectManager.StatusEffect> effects = new List<StatusEffectManager.StatusEffect>();
}
