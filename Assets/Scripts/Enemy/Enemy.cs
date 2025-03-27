using UnityEngine;
using System.Collections;
using SpawnWaves;

public class Enemy : MonoBehaviour, IEffectable
{
    [SerializeField] private Rigidbody rb;
    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;
    
    // For IEffectable interface
    Transform IEffectable.transform { get; set; }
    
    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        
        // Initialize health
        currentHealth = maxHealth;
        
        // Set transform for IEffectable
        ((IEffectable)this).transform = transform;
        
        if (PlayerMovement.Instance != null) {
            moveDirection = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerMovement.Instance != null)
        {
            transform.LookAt(PlayerMovement.Instance.transform);
            transform.position = Vector3.MoveTowards(transform.position, PlayerMovement.Instance.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    
    // Method to take damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        // Check if enemy is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // Method called when the enemy dies
    private void Die()
    {
        // // Check for poison slime death effects - we'll handle this connection differently
        // CheckSpecialDeathEffects();
        
        // Disable the enemy instead of destroying it (for object pooling)
        gameObject.SetActive(false);
    }

    // // Special effects that happen on death based on player character type
    // private void CheckSpecialDeathEffects()
    // {
    //     // In a real implementation, you would use events or a more decoupled approach
    //     // This is just a placeholder to show how you might handle poison cloud creation

    //     // Example of how to detect if player is a poison slime and create poison cloud
    //     // The actual implementation would depend on how character selection is set up
    //     if (FindObjectOfType<PoisonSlime>() != null)
    //     {
    //         // Create poison cloud at this position
    //         Debug.Log("Would create poison cloud here");
    //         // FindObjectOfType<PoisonSlime>().CreatePoisonCloud(transform.position);
    //     }
    // }

    // Return the enemy to the pool (called by PoolManager)
    public void OnDisable()
    {
        // Reset the enemy state when it's returned to the pool
        currentHealth = maxHealth;
    }
    
    // Handle collisions with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find a character component to damage
            var character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }
    }
    
    // Method to initialize the enemy (called when spawned from the pool)
    public void Initialize(float health, float speed, float attackDamage)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        moveSpeed = speed;
        damage = attackDamage;
    }
}
