using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunEvent : GameEvent
{
    public RunStatus status { get; private set; }

    public PlayerRunEvent(RunStatus status)
    {
        this.status = status;
    }
}