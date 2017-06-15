using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO(robert): Needs to be rewords so players collision box is disabled instead.
//Currently if two players are standing on top of a platform and one crouches,
//both will fall through.
[RequireComponent(typeof(EventController))]
public class TraversablePlatformTopTrigger : MonoBehaviour
{
    private static List<EventController> eventControllers = new List<EventController>();
    private bool shouldDropPlayer = false;

    void Awake()
    {
        if (eventControllers.Count == 0)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var p in players)
            {
                var eventSystem = p.GetComponent<EventController>();

                if (eventSystem != null)
                {
                    eventControllers.Add(eventSystem);
                }
            }
        }
    }

    void OnEnable()
    {
        foreach (var eventController in eventControllers)
        {
            eventController.AddListener<PlayerCrouchEvent>(OnPlayerCrouchStatusChanged);
        }
    }

    void OnDisable()
    {
        foreach (var eventController in eventControllers)
        {
            eventController.RemoveListener<PlayerCrouchEvent>(OnPlayerCrouchStatusChanged);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (shouldDropPlayer && collision.CompareTag("Player")
            && collision.gameObject.transform.position.y > transform.position.y)
        {
            foreach(var e in eventControllers)
            {
                if(e.gameObject.GetInstanceID() == collision.gameObject.GetInstanceID())
                {
                    e.Raise(new PlayerColliderStatusChangeRequestEvent(false));

                    shouldDropPlayer = false;

                    return;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var e in eventControllers)
            {
                if (e.gameObject.GetInstanceID().Equals(collision.gameObject.GetInstanceID()))
                {
                    e.Raise(new PlayerColliderStatusChangeRequestEvent(true));

                    return;
                }
            }
        }
    }

    private void OnPlayerCrouchStatusChanged(PlayerCrouchEvent e)
    {
        shouldDropPlayer = e.status.isCrouching;
    }
}
