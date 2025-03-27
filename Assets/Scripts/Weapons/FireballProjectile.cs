using UnityEngine;
using System.Collections;

namespace Weapons
{
    public class FireballProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private GameObject burnGroundEffect;
        [SerializeField] private LayerMask enemyLayerMask; // Layer mask for enemies
        
        private float damage;
        private Vector3 direction;
        private float explosionRadius;
        private float burnDamage;
        private float burnDuration;
        private bool isFlameBurst;
        private bool isMeteorDrop;
        private float burnGroundDuration;
        private GameObject player;
        
        private Rigidbody rb;
        private bool hasExploded = false;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            
            // Setup the rigidbody for projectile physics
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            // Ignore collisions with the player to prevent pushing them
            if (player != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            }
            
            Destroy(gameObject, lifetime);
        }
        
        public void Initialize(float damage, Vector3 direction, float explosionRadius, 
                              float burnDamage, float burnDuration, bool isFlameBurst, 
                              bool isMeteorDrop, float burnGroundDuration, GameObject player)
        {
            this.damage = damage;
            this.direction = direction;
            this.explosionRadius = explosionRadius;
            this.burnDamage = burnDamage;
            this.burnDuration = burnDuration;
            this.isFlameBurst = isFlameBurst;
            this.isMeteorDrop = isMeteorDrop;
            this.burnGroundDuration = burnGroundDuration;
            this.player = player;
            
            // Make sure we ignore collisions with the player
            if (player != null && GetComponent<Collider>() != null && player.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            }
            
            // Apply meteor drop properties if unlocked
            if (isMeteorDrop)
            {
                // Make it look and act more like a meteor
                transform.localScale *= 1.5f;
                rb.useGravity = true;
                
                // Add upward arc for the meteor
                rb.linearVelocity = direction * speed + Vector3.up * 8f;
            }
            else
            {
                // Normal fireball behavior
                rb.linearVelocity = direction * speed;
            }
        }
        
        // Update is called once per frame
        private void Update()
        {
            // If the projectile isn't moving, give it a push in the direction
            if (!hasExploded && rb.linearVelocity.magnitude < 1f)
            {
                rb.linearVelocity = direction * speed;
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            // Only explode if hitting an enemy or the environment, not the player
            if (collision.gameObject != player && 
                ((1 << collision.gameObject.layer) & enemyLayerMask.value) != 0 || 
                !collision.gameObject.CompareTag("Player"))
            {
                Explode();
            }
        }
        
        private void Explode()
        {
            if (hasExploded) return;
            hasExploded = true;
            
            // Stop movement
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            
            // Apply area damage - only to enemies
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                // Apply damage if damageable
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                    
                    // Apply knockback effect
                    Vector3 knockbackDirection = (hitCollider.transform.position - player.transform.position).normalized;
                    knockbackDirection.y = 0; // Keep knockback horizontal
                    
                    // The closer to the center, the stronger the knockback
                    float distanceToCenter = Vector3.Distance(transform.position, hitCollider.transform.position);
                    float knockbackForce = Mathf.Lerp(5f, 2f, distanceToCenter / explosionRadius);
                    
                    KnockbackEffect.ApplyEffect(hitCollider.gameObject, knockbackForce, knockbackDirection);
                    
                    // Apply burn status effect
                    if (burnDuration > 0)
                    {
                        // TODO: Apply burn status effect
                        // For now we'll use immediate extra damage
                        damageable.TakeDamage(burnDamage);
                    }
                }
            }
            
            // Create explosion effect
            if (explosionEffect != null)
            {
                GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosion.transform.parent = null; // Ensure it's not parented
                Destroy(explosion, 2f); // Auto-destroy after 2 seconds
            }
            
            // Create burn ground effect if Flame Burst is unlocked
            if (isFlameBurst && burnGroundEffect != null)
            {
                GameObject burnGround = Instantiate(burnGroundEffect, transform.position, Quaternion.identity);
                burnGround.transform.parent = null; // Ensure it's not parented
                Destroy(burnGround, burnGroundDuration);
            }
            
            // Handle meteor effect (rolling fireball)
            if (isMeteorDrop)
            {
                StartCoroutine(RollMeteor());
            }
            else
            {
                // Normal fireball just destroys itself and all children
                DestroyFireballCompletely();
            }
        }
        
        // Helper method to destroy the fireball completely
        private void DestroyFireballCompletely()
        {
            // First, detach any child objects that need to persist (like effects)
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                
                // Check if this is a persistent effect that should survive
                ParticleSystem ps = child.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    // Make the particle system independent and destroy it when finished
                    child.parent = null;
                    ps.Stop(true); // Stop emitting but let existing particles live out their life
                    var main = ps.main;
                    Destroy(child.gameObject, main.duration + main.startLifetime.constantMax);
                }
                else
                {
                    // If it's not a particle effect, just destroy it immediately
                    Destroy(child.gameObject);
                }
            }
            
            // Finally destroy the fireball itself
            Destroy(gameObject);
        }
        
        private IEnumerator RollMeteor()
        {
            // Create a new object for the rolling meteor
            GameObject rollingMeteor = new GameObject("RollingMeteor");
            rollingMeteor.transform.position = transform.position;
            rollingMeteor.transform.rotation = transform.rotation;
            rollingMeteor.transform.parent = null; // Ensure it's not parented
            
            // Add a sphere collider for rolling
            SphereCollider sphereCollider = rollingMeteor.AddComponent<SphereCollider>();
            sphereCollider.radius = 0.5f;
            
            // Add rigidbody for physics
            Rigidbody meteorRb = rollingMeteor.AddComponent<Rigidbody>();
            meteorRb.mass = 5f;
            
            // Ignore collision with player
            Physics.IgnoreCollision(sphereCollider, player.GetComponent<Collider>(), true);
            
            // Add visual
            GameObject visual = Instantiate(gameObject, rollingMeteor.transform);
            // Only use the visual component, disable its rigidbody, etc
            Rigidbody visualRb = visual.GetComponent<Rigidbody>();
            if (visualRb != null)
            {
                visualRb.isKinematic = true;
                Destroy(visualRb);
            }
            Collider visualCollider = visual.GetComponent<Collider>();
            if (visualCollider != null)
            {
                visualCollider.enabled = false;
                Destroy(visualCollider);
            }
            
            // Remove the FireballProjectile script from the visual
            Destroy(visual.GetComponent<FireballProjectile>());
            
            visual.transform.localPosition = Vector3.zero;
            
            // Calculate roll direction (opposite of player direction)
            Vector3 rollDirection = direction;
            meteorRb.AddForce(rollDirection * 10f, ForceMode.Impulse);
            
            // Leave fire trail behind
            float rollDuration = 3f;
            float timeElapsed = 0f;
            float fireTrailInterval = 0.2f;
            float lastFireTrail = 0f;
            
            while (timeElapsed < rollDuration)
            {
                timeElapsed += Time.deltaTime;
                
                // Create fire trail at intervals
                if (timeElapsed - lastFireTrail > fireTrailInterval)
                {
                    lastFireTrail = timeElapsed;
                    
                    if (burnGroundEffect != null)
                    {
                        GameObject burnGround = Instantiate(burnGroundEffect, rollingMeteor.transform.position, Quaternion.identity);
                        burnGround.transform.parent = null; // Ensure it's not parented
                        Destroy(burnGround, burnGroundDuration * 0.5f);
                    }
                    
                    // Damage enemies in the path - only enemies
                    Collider[] hitColliders = Physics.OverlapSphere(rollingMeteor.transform.position, 1f, enemyLayerMask);
                    foreach (var hitCollider in hitColliders)
                    {
                        IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(damage * 0.5f);
                        }
                    }
                }
                
                yield return null;
            }
            
            // Final explosion at end of roll - only enemies
            Collider[] finalHitColliders = Physics.OverlapSphere(rollingMeteor.transform.position, explosionRadius * 0.5f, enemyLayerMask);
            foreach (var hitCollider in finalHitColliders)
            {
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage * 0.5f);
                }
            }
            
            if (explosionEffect != null)
            {
                GameObject explosion = Instantiate(explosionEffect, rollingMeteor.transform.position, Quaternion.identity);
                explosion.transform.parent = null; // Ensure it's not parented
                Destroy(explosion, 2f); // Auto-destroy after 2 seconds
            }
            
            // Destroy all children of rolling meteor
            for (int i = rollingMeteor.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(rollingMeteor.transform.GetChild(i).gameObject);
            }
            
            Destroy(rollingMeteor);
            DestroyFireballCompletely();
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
} 