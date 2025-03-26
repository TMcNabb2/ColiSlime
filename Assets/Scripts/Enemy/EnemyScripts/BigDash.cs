using System;
using UnityEngine;

public class BigDash : MonoBehaviour
{
    public float attackTime;
    public float chargeTime;
    public Enemy myAI;


    private float moveForce;
    private bool _inTarget;
    private float currentchargeTime;
    private float currentInTargetTime;
    

    private void Update()
    {
        if (_inTarget)
        {
            if (currentInTargetTime >= attackTime)
            {
                myAI.enabled = false;
                transform.parent.transform.LookAt(PlayerMovement.Instance.transform);
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

        if (moveForce < 0.4)
        {
            myAI.enabled = true;
        }
        
        transform.parent.transform.Translate(transform.forward * moveForce * Time.deltaTime);
        
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
