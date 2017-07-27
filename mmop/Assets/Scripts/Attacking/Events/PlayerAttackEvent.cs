/// <summary>
/// Local character event.
/// 
/// Raised: when a characters attack status has changed, for example when they: stop attacking, start attacking primary, or start attacking seconday.
///
/// Data: the characters AttackStatus at time of event raised.
/// </summary>
public class PlayerAttackEvent : GameEvent
{
    /// <summary>
    /// Stores the attack status at the frame when the event was raised.
    /// </summary>
    public AttackStatus status { get; private set; }

    /// <summary>
    /// Constructor. AttackStatus is expected not to be null so must be set in constructor.
    /// </summary>
    /// <param name="status">AttackStatus at time of event.</param>
    public PlayerAttackEvent(AttackStatus status)
    {
        this.status = status;
    }
}
