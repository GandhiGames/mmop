using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRigidbodyMotor : MonoBehaviour, PlayerMotor
{
    public Vector2 velocity { get { return rigidbody2d.velocity; } set { rigidbody2d.velocity = value; } }

    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();    
    }
    
    public void AddForce(Vector2 force)
    {
        rigidbody2d.AddForce(force);
    }
}
