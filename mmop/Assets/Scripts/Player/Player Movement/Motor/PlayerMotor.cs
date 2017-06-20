using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerMotor
{
    Vector2 velocity { get; set; }

    void AddForce(Vector2 force);
}
