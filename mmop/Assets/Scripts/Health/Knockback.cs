using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class Knockback : MonoBehaviour, Damageable
{
    public float knockbackForce = 100f;

    private PlayerMotor motor;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();    
    }

    public void Damage(float amount, Vector2 dir)
    {
        motor.AddForce(dir * knockbackForce);
    }

}
