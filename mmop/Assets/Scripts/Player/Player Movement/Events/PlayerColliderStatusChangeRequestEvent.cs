using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderStatusChangeRequestEvent : GameEvent
{
    public bool shouldEnable { get; private set; }

    public PlayerColliderStatusChangeRequestEvent(bool shouldEnable)
    {
        this.shouldEnable = shouldEnable;
    }
}