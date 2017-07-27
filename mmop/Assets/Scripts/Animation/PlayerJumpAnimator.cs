using UnityEngine;

/// <summary>
/// Updates character jump animation status. Listens for PlayerJumpEvent.
/// </summary>
[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerJumpAnimator : MonoBehaviour
{
    /// <summary>
    /// Generate jump animation property id from string. Pre-calculating this hash is faster than using string literal each time 
    /// you want to update an animation.
    /// </summary>
    private static readonly int JUMP_HASH = Animator.StringToHash("Jump");

    private Animator animator;
    private EventController eventController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        eventController = GetComponent<EventController>();
    }

    private void OnEnable()
    {
        eventController.AddListener<PlayerJumpEvent>(AnimateJump);
    }

    private void OnDisable()
    {
        eventController.RemoveListener<PlayerJumpEvent>(AnimateJump);
    }

    /// <summary>
    /// Updates jump animation hash when PlayerJumpEvent received.
    /// </summary>
    /// <param name="e">The event raised when the player has jumped.</param>
    private void AnimateJump(PlayerJumpEvent e)
    {
        animator.SetTrigger(JUMP_HASH);
    }
}
