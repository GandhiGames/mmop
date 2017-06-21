using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController), typeof(Animator))]
public class PlayerGroundAnimator : MonoBehaviour
{
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
    /// <param name="e"></param>
    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        animator.SetBool(GROUNDED_HASH, e.groundStatus == GroundStatus.Grounded);
    }
}
