using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallStatusEventChangeEvent : GameEvent
{
    public bool isTouchingWall { get; private set; }

    public PlayerWallStatusEventChangeEvent(bool isTouchingWall)
    {
        this.isTouchingWall = isTouchingWall;
    }
}
