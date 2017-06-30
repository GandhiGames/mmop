using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: do we want to re-add the ability to fall through the platform if you are crouching in-air?
[RequireComponent(typeof(EventController))]
public class TraversablePlatformTopTrigger : MonoBehaviour
{
    /// <summary>
    /// This is the maximum time a player is given between crouch presses before they are registered 
    /// and the player is dropped through the platform. If you are finding it too easy/hard to drop through
    /// a platform, adjusting this value should help.
    /// </summary>
    public float timeBetweenCrouchPressed = 0.5f;

    /// <summary>
    /// This is everything that the platform needs to know about a player.
    /// </summary>
    private class CollidingPlayer
    {
        public EventController events;
        public PlayerControls controls;
        public float timeOfLastCrouchPress;

        public CollidingPlayer(EventController events, PlayerControls controls)
        {
            this.events = events;
            this.controls = controls;
            timeOfLastCrouchPress = 0f;
        }
    }

    // We store the instance id of each colliding player so we can quickly check that we have not already added a player to our collection.
    private Dictionary<int, CollidingPlayer> players = new Dictionary<int, CollidingPlayer>();

    void Update()
    {
        // Early out for when no player are colliding with the platform.
        if (players.Count == 0)
        {
            return;
        }

 
        foreach (var p in players)
        {
            if (p.Value.controls.IsCrouchButtonPressed())
            {
                if (p.Value.timeOfLastCrouchPress >= (Time.time - timeBetweenCrouchPressed))
                {
                    // We now know that the player has pressed crouch twice in quick succession so we
                    // can drop the player through the platform.
                    p.Value.events.Raise(new PlayerColliderStatusChangeRequestEvent(false));
                }

                // Reset the time counter for last key press as the player has just pressed the crouch key.
                p.Value.timeOfLastCrouchPress = Time.time;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // We don't want to re-add players that are already in our collection so we first check if the
        // dictionary contains the players transform id.
        if (collision.CompareTag("Player") && !players.ContainsKey(collision.gameObject.GetInstanceID()))
        {
            var events = collision.GetComponent<EventController>();
            var controller = collision.GetComponent<PlayerControls>();

            if(events == null || controller == null)
            {
                // This platform cannot work as intended if it cannot access the controls and event system of a player.
                Debug.LogWarning("EventController and/or PlayerControls have not been found on an object with the tag 'Player', this object will not be able" +
                    " to fall through this platform");
            }
            else
            {
                // We add the colliding player to our list to perform future checks to see whether we should drop the player through the platform.
                players.Add(collision.gameObject.GetInstanceID(), new CollidingPlayer(events, controller));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // We remove the player from our list as we are no longer interested in listening to that players controls. 
            // They will be re-added if they collide with us in future.
            players.Remove(collision.gameObject.GetInstanceID());
        }
    }
}
