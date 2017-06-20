using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: remove this system and replace with events.
public interface Damageable 
{
    void Damage(float amount, Vector2 dir);
}
