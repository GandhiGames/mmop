using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversablePlatformBottomTrigger : MonoBehaviour
{
    private static List<EventController> eventControllers = new List<EventController>();

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.gameObject.transform.position.y < transform.position.y)
        {
            foreach (var e in eventControllers)
            {
                if (e.gameObject.GetInstanceID() == collision.gameObject.GetInstanceID())
                {
                    e.Raise(new PlayerColliderStatusChangeRequestEvent(false));

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
}
