using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchEvent : GameEvent
{
    public CrouchStatus status { get; private set; }

    public PlayerCrouchEvent(CrouchStatus controller)
    {
        status = controller;
    }
    
}
