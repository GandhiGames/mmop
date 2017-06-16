using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClingEvent : GameEvent
{
    public WallClingStatus status;

    public PlayerWallClingEvent(WallClingStatus status)
    {
        this.status = status;
    }
}
