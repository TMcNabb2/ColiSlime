using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;


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
            transform.LookAt(PlayerMovement.Instance.transform);
            transform.position = Vector3.MoveTowards(transform.position, PlayerMovement.Instance.transform.position, moveSpeed * Time.deltaTime);
        }
    }

}
