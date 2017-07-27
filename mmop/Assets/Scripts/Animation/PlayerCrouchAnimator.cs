using UnityEngine;

/// <summary>
/// Updates characters crouch animation status. Listens for PlayerCrouchEvent.
/// </summary>
[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerCrouchAnimator : MonoBehaviour
{
    /// <summary>
    /// Generate crouch animation property id from string. Pre-calculating this hash is faster than using string literal each time 
    /// you want to update an animation.
    /// </summary>
    private static readonly int CROUCH_HASH = Animator.StringToHash("Crouching");

    private Animator animator;
    private EventController eventController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        eventController = GetComponent<EventController>();
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerCrouchEvent>(AnimateCrouch);
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerCrouchEvent>(AnimateCrouch);
    }

    /// <summary>
    /// Upodates animation based on current crouch status.
    /// </summary>
    /// <param name="e">The event raised when the characters crouch status has changed.</param>
    private void AnimateCrouch(PlayerCrouchEvent e)
    {
        animator.SetBool(CROUCH_HASH, e.status.isCrouching);
    }
}
