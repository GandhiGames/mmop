/// <summary>
/// Character event.
/// 
/// Raised: when a character successfully attacks another character.
/// 
/// Data: The EventController of the character that was hit.
/// </summary>
public class DamageGivenEvent : GameEvent
{
    /// <summary>
    /// The EventController of the character that was hit.
    /// </summary>
    public EventController enemyEvents { get; private set; }

    /// <summary>
    /// Constructor. EventController is expected to not be null.
    /// </summary>
    /// <param name="enemyEvents">EventController of character that was hit.</param>
    public DamageGivenEvent(EventController enemyEvents)
    {
        this.enemyEvents = enemyEvents;
    }
}
