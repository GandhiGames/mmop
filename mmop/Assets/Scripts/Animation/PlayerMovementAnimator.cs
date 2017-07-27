using UnityEngine;

//TODO(robert): should this be seperated into two classes: calculation / updatin
//or should we create a seperate event for movement speed changed.

/// <summary>
/// Calculates the characters horizontal movement velocity and updates the animation speed.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerMovementAnimator : MonoBehaviour
{
    /// <summary>
    /// Generate speed animation property id from string. Pre-calculating this hash is faster than using string literal each time 
    /// you want to update an animation.
    /// </summary>
    private static readonly int SPEED_HASH = Animator.StringToHash("Speed");

    private Animator animator;

    /// <summary>
    /// Stores the characters x position from the previous frame. Used to calculate
    /// the characters velocity since last frame.
    /// </summary>
    private float prevHorPos;

    void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    void Start()
    {
        prevHorPos = transform.position.x;    
    }

    /// <summary>
    /// Calculates horizontal velocity and updates animator.
    /// </summary>
    void FixedUpdate()
    {
        var velocity = (transform.position.x - prevHorPos) / Time.deltaTime;
        prevHorPos = transform.position.x;

        animator.SetFloat(SPEED_HASH, Mathf.Abs(velocity));
    }
}
