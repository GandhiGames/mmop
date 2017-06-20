using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType
{
    Primary = 0,
    Secondary,
    None = 100
}

public interface AttackStatus
{
    PlayerAttackType attackType { get; }
}

[RequireComponent(typeof(PlayerControls), typeof(EventController))]
public class PlayerAttackController : MonoBehaviour, AttackStatus
{
    public PlayerAttackType attackType { get; private set; }

    private delegate bool attackButtonHeldDelegate();
    private static Dictionary<PlayerAttackType, attackButtonHeldDelegate> attackLookUp = new Dictionary<PlayerAttackType, attackButtonHeldDelegate>();

    private PlayerControls playerControls;
    private EventController eventController;
    private PlayerAttackEvent attackEvent;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        attackType = PlayerAttackType.None;
        attackEvent = new PlayerAttackEvent(this);

        if(attackLookUp.Count == 0)
        {
            attackLookUp.Add(PlayerAttackType.Primary, playerControls.IsPrimaryAttackButtonHeld);
            attackLookUp.Add(PlayerAttackType.Secondary, playerControls.IsSecondaryAttackButtonHeld);
        }
    }

    void Update()
    {
        bool attackRegistered = false;

        foreach(var attackCheck in attackLookUp)
        {
            if(attackCheck.Value())
            {
                RegisterAttack(attackCheck.Key);
                attackRegistered = true;
                break;
            }
        }

        if(!attackRegistered)
        {
            RegisterAttack(PlayerAttackType.None);

        }
    }

    private void RegisterAttack(PlayerAttackType attackTypeToCheck)
    {
        if(attackType != attackTypeToCheck)
        {
            attackType = attackTypeToCheck;
            eventController.Raise(attackEvent);
        }
    }
}
