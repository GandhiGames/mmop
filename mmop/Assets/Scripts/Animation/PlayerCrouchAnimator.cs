using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerCrouchAnimator : MonoBehaviour
{
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

    private void AnimateCrouch(PlayerCrouchEvent e)
    {
        animator.SetBool("Crouching", e.status.isCrouching);
    }
}
