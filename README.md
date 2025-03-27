# ColiSlime
ColiSlime Prototype - Survivor Style Game

## Game Overview
ColiSlime is a survivor-style game where you control different types of slimes with unique abilities to fight against waves of enemies. The game features an auto-targeting weapon system with upgradable abilities and different character types to choose from.

## Character Types
1. **Speed Slime**: Moves faster but has less health.
   - 15% Increase to movement speed, but 15% less max HP
   - Unique: Cooldowns reduced by 1% per level

2. **Tank Slime**: Higher health and defense but slower movement.
   - 30% increased Max HP, 15% reduced movement speed
   - Unique: Reduces damage taken by 50% for 5 seconds after being hit

3. **Elemental Slime**: Deals bonus elemental damage.
   - Attacks apply a random effect (Fire-Burn, Ice-Slow, Lightning-Paralyze)
   - Unique: 5% Chance to recast the last used ability

4. **Shadow Slime**: Gains evasive abilities.
   - 10% increased Dodge and 10% increased movement speed
   - Unique: Dodge increased by 50% for 5s after getting hit

5. **Balanced Slime**: A jack-of-all-trades option.
   - 15% Increase to XP gathered 
   - Unique: Adaptation – 10% chance to choose another ability on level up

6. **Poison Slime**: A damage over time focused slime.
   - All attacks apply a stacking poison effect (damage over time, 5 max stacks)
   - Unique: Toxicity – Killing an enemy leaves a poison cloud that damages all enemies in a radius

## Weapon System
The game features an auto-targeting weapon system with upgradable abilities. The current implementation includes:

### Fire Weapon
- **Flame Shot**: Shoots a fireball that explodes on impact, burning enemies
- **Flame Burst**: Increases the impact area of the fireball and leaves a patch of burning ground behind
- **Molten Core**: Doubles the effects of fire abilities (2 Fireballs, double DOT dmg)
- **Meteor Drop**: Fireballs transform into meteors that drop onto enemies and roll away from you, dealing damage in a line and leaving fire behind
- **Ultimate - Hellscape**: Unleashes a rain of meteors, setting the screen ablaze for 5s

## Setup Instructions

1. **Create Base Scriptable Objects**:
   - Create a new CharacterBaseStats asset for each character type (Assets > Create > Character > CharacterStats)
   - Configure the base stats for each character type
   - Create EnemyDataAsset objects for each enemy type

2. **Scene Setup**:
   - Create a new scene named "GameScene"
   - Add a ground plane (or your terrain)
   - Create a Player prefab with the following components:
     - Character script (or one of the specific character types)
     - Rigidbody
     - Collider
     - PlayerMovement script
   - Create weapon prefabs with their respective Weapon scripts

3. **Manager Objects**:
   - Create an empty GameObject named "Managers" in the scene
   - Add the following components to it:
     - CharacterSelection
     - WeaponManager
     - GameManager
     - PoolManager (configure prefabs in the SerializedPrefabPool array)
     - SpawnWaveManager (configure EnemyDataAsset references)
   - Configure the references in the Inspector for each manager

4. **Pool Setup**:
   - Configure the PoolManager with SerializedPrefabPool entries for:
     - Fireball prefab
     - Meteor prefab
     - Enemy prefabs
     - Effect prefabs

5. **Status Effects**:
   - Create prefabs for each status effect (BurningEffect, PoisonEffect)
   - Create an EffectReferences asset and populate it with references to the effect prefabs
   - Ensure the prefabs are set up in the PoolManager

6. **Enemy Setup**:
   - Create enemy prefabs with the Enemy script
   - Set up SpawnWaveManager with EnemyDataAsset references
   - Use WaveSpawner to handle enemy spawning

7. **Stats System**:
   - Each character type uses the JDoddsNAIT.Stats system
   - Create CharacterBaseStats ScriptableObjects for each character type
   - Stat modifiers are applied through the Character class helpers

8. **Testing**:
   - Press Play to test the game
   - The character should automatically attack nearby enemies
   - Use WASD to move and Space to dash

## Additional Notes
- Make sure all prefabs have the appropriate tags (Player, Enemy, Ground)
- Set up layers properly for collision detection
- Status effects need to be referenced in the EffectReferences asset
- The PoolManager handles object pooling for all prefabs
- Make sure to assign the appropriate Character Scripts to player prefabs:
  - SpeedSlime
  - TankSlime
  - ElementalSlime
  - ShadowSlime
  - BalancedSlime
  - PoisonSlime

## Troubleshooting
- If objects aren't being pooled properly, check the PoolManager SerializedPrefabPool setup
- If status effects aren't being applied, check that the EffectReferences asset is properly set up
- If characters aren't getting the right stat modifications, check their CharacterBaseStats ScriptableObject
