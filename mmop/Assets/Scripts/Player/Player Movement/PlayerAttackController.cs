using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AttackStatus
{
    bool isAttackingPrimary { get; }
    bool isAttackingSecondary { get; }
}

[RequireComponent(typeof(PlayerControls), typeof(EventController))]
public class PlayerAttackController : MonoBehaviour, AttackStatus
{
    public bool isAttackingPrimary { get; private set; }
    public bool isAttackingSecondary { get; private set; }

    private PlayerControls playerControls;
    private EventController eventController;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        isAttackingPrimary = false;
        isAttackingSecondary = false;
    }

    void Update()
    {
        bool shouldAttackPrimary = playerControls.IsPrimaryAttackButtonHeld();

        if (shouldAttackPrimary != isAttackingPrimary)
        {
            isAttackingPrimary = shouldAttackPrimary;
            eventController.Raise(new PlayerAttackEvent(this));
        }

        bool shouldAttackSecondary = playerControls.IsSecondaryAttackButtonHeld();

        if(shouldAttackSecondary != isAttackingSecondary)
        {
            isAttackingSecondary = shouldAttackSecondary;
            eventController.Raise(new PlayerAttackEvent(this));
        }
    }
}
