using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerAttackAnimator : MonoBehaviour
{
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

    private void AnimatePrimaryAttack(PlayerAttackEvent e)
    {
        animator.SetBool("Primary Attack", e.status.isAttackingPrimary);
        animator.SetBool("Secondary Attack", e.status.isAttackingSecondary);
    }
}