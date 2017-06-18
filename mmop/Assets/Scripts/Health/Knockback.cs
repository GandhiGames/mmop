using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour, Damageable
{
    public float knockbackForce = 100f;

    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();    
    }

    public void Damage(float amount, Vector2 dir)
    {
        rigidbody2d.AddForce(dir * knockbackForce);
    }

}
