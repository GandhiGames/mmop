﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerAttackAnimator : MonoBehaviour
{
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

    private void AnimatePrimaryAttack(PlayerAttackEvent e)
    {
        animator.SetBool(PRIMARY_ATTACK_HASH, e.status.attackType == PlayerAttackType.Primary);
        animator.SetBool(SECONDARY_ATTACK_HASH, e.status.attackType == PlayerAttackType.Secondary);
    }
}