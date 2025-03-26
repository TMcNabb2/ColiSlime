using System;
using UnityEngine;

public class MediumEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    private Vector3 _lastPlayerPosition;

    private void Awake()
    {
        if(PlayerMovement.Instance != null){
            moveDirection = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerMovement.Instance != null)
        {
            if ((transform.position - PlayerMovement.Instance.transform.position).magnitude > 5f)
            {
                // move towards in zigzag pattern
                Vector3 directionToPlayer = (PlayerMovement.Instance.transform.position - transform.position).normalized;
                float sineFactor = Mathf.Sin(Time.time * (moveSpeed/2)) * 1;
                Vector3 sineOffset = Vector3.Cross(directionToPlayer, Vector3.up).normalized * sineFactor;
                Vector3 finalDirection = directionToPlayer + sineOffset;

                // Move towards the player with the adjusted direction
                transform.LookAt(transform.position + finalDirection);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + finalDirection, moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 nextPosition = (PlayerMovement.Instance.transform.position - _lastPlayerPosition).normalized;
                nextPosition *= Mathf.Abs((_lastPlayerPosition - PlayerMovement.Instance.transform.position).magnitude + 3f);
                nextPosition += PlayerMovement.Instance.transform.position;
                // predict and move ahead
                transform.LookAt(nextPosition);
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
            }
            
            _lastPlayerPosition = PlayerMovement.Instance.transform.position;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == PlayerMovement.Instance.gameObject)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (other.gameObject.TryGetComponent<IDamageable>(out damageable))
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
           
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 3));
    }
}
