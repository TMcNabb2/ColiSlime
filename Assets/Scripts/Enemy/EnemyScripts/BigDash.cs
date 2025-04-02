using System;
using UnityEngine;

public class BigDash : MonoBehaviour
{
    public float attackTime;
    public float chargeTime;
    public Enemy myAI;


    private float moveForce;
    [SerializeField]
    private bool _inTarget;
    private float currentchargeTime;
    private float currentInTargetTime;
    Vector3 directionToPlayer;



    private void Update()
    {
        if (_inTarget)
        {
            if (currentInTargetTime >= attackTime)
            {

                directionToPlayer = (new Vector3(PlayerMovement.Instance.transform.position.x, transform.position.y, PlayerMovement.Instance.transform.position.z) - transform.position).normalized;
                directionToPlayer.y = 0;
                if (currentchargeTime >= chargeTime)
                {
                    // attack
                    
                    moveForce = 80;
                    currentchargeTime = 0f;
                    currentInTargetTime = 0f;
                }
                currentchargeTime += Time.deltaTime;
            }
            currentInTargetTime += Time.deltaTime;
        }
        else

        if (moveForce < 2)
        {

            directionToPlayer = Vector3.zero;
        }
        
        
        transform.parent.transform.Translate(directionToPlayer * moveForce * Time.deltaTime,Space.World);
        
        moveForce *= 0.95f;

    }

    private void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == PlayerMovement.Instance.gameObject.transform)
        {
            _inTarget = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == PlayerMovement.Instance.gameObject.transform)
        {
            _inTarget = false;
            currentInTargetTime = 0;
            currentchargeTime = 0;
        }
    }
}
