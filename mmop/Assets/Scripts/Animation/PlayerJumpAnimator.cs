using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(EventController))]
public class PlayerJumpAnimator : MonoBehaviour
{
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

    private void AnimateJump(PlayerJumpEvent e)
    {
        animator.SetTrigger("Jump");
    }
}
