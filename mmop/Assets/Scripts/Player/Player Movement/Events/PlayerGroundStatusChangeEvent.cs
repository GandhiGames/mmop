using UnityEngine;

/// <summary>
/// Stores the characters ground status. 
/// 
/// NotGrounded: Not touching or near ground.
/// Grounded: Touching ground.
/// AlmostGrounded: Player is falling towards ground and has almost touched ground. For more information on this, see: GroundCheck::almostGroundTransform and PlayerJumpController::Update.
/// None: Initial ground status. If receieved, the players ground status could not be calculated.
/// 
/// 
/// </summary>
public enum GroundStatus
{
    NotGrounded = 0,
    Grounded,
    AlmostGrounded,
    None = 100
}

/// <summary>
/// Local Character Event:
/// 
/// Raised: when a characters ground status has changed. See GroundStatus for more information.
/// 
/// Data: the characters GroundStatus at time the event was raised.
/// </summary>
public class PlayerGroundStatusChangeEvent : GameEvent
{
    /// <summary>
    /// Stores the ground status at the frame when the event was raised.
    /// </summary>
    public GroundStatus groundStatus { get; private set; }

    /// <summary>
    /// Can be null. If a characters ground status is GroundStatus::Grounded or GroundStatus::AlmostGrounded 
    /// then this will store the object the player is using as ground.
    /// </summary>
    public GameObject ground { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groundStatus"></param>
    /// <param name="ground"> Defaults to null.</param>
    public PlayerGroundStatusChangeEvent(GroundStatus groundStatus, GameObject ground = null)
    {
        this.groundStatus = groundStatus;

        // We want to ensure that the ground object is not null if the character is near or touching the ground.
        if ((groundStatus == GroundStatus.Grounded || groundStatus == GroundStatus.AlmostGrounded) && ground == null)
        {
            Debug.LogWarning("Ground should not be null if player near or touching ground.");
        }

        this.ground = ground;
    }
}
