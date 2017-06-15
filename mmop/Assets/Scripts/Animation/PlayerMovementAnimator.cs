using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovementAnimator : MonoBehaviour
{
    private Animator animator;
    private float prevHorVel;

    void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    void Start()
    {
        prevHorVel = transform.position.x;    
    }

    void FixedUpdate()
    {
        var velocity = (transform.position.x - prevHorVel) / Time.deltaTime;
        prevHorVel = transform.position.x;

        animator.SetFloat("Speed", Mathf.Abs(velocity));
    }
}
