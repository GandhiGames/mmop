//TODO: normalise whether using interface and passing in class or
// having standard variables in events.

/// <summary>
/// Local character event.
/// 
/// Raised: when a character either starts or stops crouching. The event is only raised on the frame that a characters
/// crouch status changes, for example: when you receieve a crouch event that states the character is crouching, you can safely assume
/// that the characters continues to crouch until you receive a PlayerCrouchEvent that states otherwise.
/// 
/// Data: stores current CrouchStatus at time of event raised.
/// </summary>
public class PlayerCrouchEvent : GameEvent
{
    /// <summary>
    /// Stores the crouch status at the frame when the event was raised.
    /// </summary>
    public CrouchStatus status { get; private set; }

    /// <summary>
    /// Constructor. CrouchStatus is expected not to be null so must be set in constructor.
    /// </summary>
    /// <param name="controller">CrouchStatus at time of event.</param>
    public PlayerCrouchEvent(CrouchStatus controller)
    {
        status = controller;
    }
    
}
