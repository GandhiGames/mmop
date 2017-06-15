using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpEvent : GameEvent
{
    public WallJumpStatus status;

    public WallJumpEvent(WallJumpStatus status)
    {
        this.status = status;
    }
}
