using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float secondsBetweenPoints = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pointA.position, pointB.position,
                                            Mathf.SmoothStep(0f, 1f,
                                            Mathf.PingPong(Time.time / secondsBetweenPoints, 1f)
                                          ));
    }
}
