using UnityEngine;

public class AnimationOnMovementSpeed : MonoBehaviour
{
    public Animator animator;
    public float multiplier = 1;
    private Vector3 lastPos;
    private void Update()
    {
        float speed = (Mathf.Abs((lastPos - transform.position).magnitude) /Time.deltaTime) * multiplier;

        animator.speed = speed;


        lastPos = transform.position;
    }
}
