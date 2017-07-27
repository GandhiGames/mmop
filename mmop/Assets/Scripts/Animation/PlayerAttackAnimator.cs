using UnityEngine;

/// <summary>
/// Listens and responds to attack events. Responsible for updating the characters animation based on attack status.
/// </summary>
[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerAttackAnimator : MonoBehaviour
{
    /// <summary>
    /// Generate primary/secondary attack animation property ids from string. Pre-calculating this hash is faster than using string literal each time 
    /// you want to update an animation.
    /// </summary>
    private static readonly int PRIMARY_ATTACK_HASH = Animator.StringToHash("Primary Attack");
    private static readonly int SECONDARY_ATTACK_HASH = Animator.StringToHash("Secondary Attack");

    private Animator animator;
    private EventController eventController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        eventController = GetComponent<EventController>();
    }

    private void OnEnable()
    {
        eventController.AddListener<PlayerAttackEvent>(AnimatePrimaryAttack);
    }

    private void OnDisable()
    {
        eventController.RemoveListener<PlayerAttackEvent>(AnimatePrimaryAttack);
    }

    /// <summary>
    /// Updates animation based on current character attack status.
    /// </summary>
    /// <param name="e">The event raised when the characters attack status has changed.</param>
    private void AnimatePrimaryAttack(PlayerAttackEvent e)
    {
        animator.SetBool(PRIMARY_ATTACK_HASH, e.status.attackType == PlayerAttackType.Primary);
        animator.SetBool(SECONDARY_ATTACK_HASH, e.status.attackType == PlayerAttackType.Secondary);
    }
}