using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: normalise whether using interface and passing in class or
// having standard variables in events.
public class PlayerCrouchEvent : GameEvent
{
    public CrouchStatus status { get; private set; }

    public PlayerCrouchEvent(CrouchStatus controller)
    {
        status = controller;
    }
    
}
