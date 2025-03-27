using UnityEngine;
using JDoddsNAIT.Stats;


    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/Character/CharacterStats")]
    public class CharacterStats : ScriptableObject, IStatContainer
    {
        [Header("Character Info")]
        public string characterName = "Default Character";
        public Sprite characterSprite;
        public string description = "Default character description";

        [Header("Base Stats")]
        public float maxHealth = 100f;
        public float movementSpeed = 5f;
        public float xpMultiplier = 1f;

        [Header("Special Abilities")]
        public bool hasAdaptation = false;
        public float adaptationChance = 0f; // Percentage chance (0-1)

        public Query GetBaseValue(string statName)
        {
            return statName switch
            {
                "MaxHealth" => new Query(statName, maxHealth),
                "MovementSpeed" => new Query(statName, movementSpeed),
                "XPMultiplier" => new Query(statName, xpMultiplier),
                "AdaptationChance" => new Query(statName, adaptationChance),
                _ => null
            };
        }
    }
 