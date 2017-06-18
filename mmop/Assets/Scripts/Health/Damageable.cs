using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable 
{
    void Damage(float amount, Vector2 dir);
}
