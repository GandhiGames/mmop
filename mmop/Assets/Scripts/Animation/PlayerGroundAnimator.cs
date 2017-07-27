using UnityEngine;

/// <summary>
/// Updates characters ground animation status. 
/// 
/// Various animations rely on the grounded bool, for example: different attack animations are played
/// depending on whether the player is touching the ground or not.
/// </summary>
[RequireComponent(typeof(EventController), typeof(Animator))]
public class PlayerGroundAnimator : MonoBehaviour
{
    /// <summary>
    /// Generate grounded animation property ids from string. Pre-calculating this hash is faster than using string literal each time 
    /// you want to update an animation.
    /// </summary>
    private static readonly int GROUNDED_HASH = Animator.StringToHash("Grounded");

    private EventController events;
    private Animator animator;

    void Awake()
    {
        events = GetComponent<EventController>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        events.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
    }

    void OnDisable()
    {
        events.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
    }

    /// <summary>
    /// Listens for ground status changes i.e. if the player is grounded or not
    /// and updates the animation parameter accordingly.
    /// </summary>
    /// <param name="e">The event raised when the players grounded status changes.</param>
    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        animator.SetBool(GROUNDED_HASH, e.groundStatus == GroundStatus.Grounded);
    }
}
