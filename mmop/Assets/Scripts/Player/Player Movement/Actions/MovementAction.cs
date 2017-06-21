using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : MonoBehaviour
{
    /// <summary>
    /// Check if any other movement actions have been added to the player and warn user accordingly.
    /// We do not want (as an example), a player to be able to use a grappling hook and teleport at the same time.
    /// </summary>
    protected virtual void Awake()
    {
        var otherMoveActions = GetComponents<MovementAction>();

        if (otherMoveActions.Length > 1)
        {
            Debug.LogWarning("Only one movement action should be attached to a character");
        }
    }
}
