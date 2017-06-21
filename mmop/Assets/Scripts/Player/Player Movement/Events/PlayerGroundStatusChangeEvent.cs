using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundStatus
{
    NotGrounded = 0,
    Grounded,
    AlmostGrounded,
    None = 100
}

public class PlayerGroundStatusChangeEvent : GameEvent
{
    public GroundStatus groundStatus { get; private set; }
    public GameObject ground { get; private set; }

    public PlayerGroundStatusChangeEvent(GroundStatus groundStatus, GameObject ground)
    {
        this.groundStatus = groundStatus;
        this.ground = ground;
    }
}
