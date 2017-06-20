using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, PlayerControls
{
    public string controllerName = "Keyboard";

    private static readonly string JUMP = " Jump";
    private static readonly string CROUCH = " Crouch";
    private static readonly string MOVEMENT_ACTION = " Movement Action";
    private static readonly string PRIMARY_ATTACK = " Primary Attack";
    private static readonly string SECONDARY_ATTACK = " Secondary Attack";
    private static readonly string MOVE_LEFT = " Move Left";
    private static readonly string MOVE_RIGHT = " Move Right";
    private static readonly string AXIS_HORIZONTAL = " Horizontal";

    public bool IsJumpButtonPressed()
    {
        return Input.GetButtonDown(controllerName + JUMP);
    }

    public bool IsJumpButtonHeld()
    {
        return Input.GetButton(controllerName + JUMP);
    }

    public bool IsJumpButtonReleased()
    {
        return Input.GetButtonUp(controllerName + JUMP);
    }

    public bool IsCrouchButtonPressed()
    {
        return Input.GetButtonDown(controllerName + CROUCH);
    }

    public bool IsCrouchButtonHeld()
    {
        return Input.GetButton(controllerName + CROUCH);
    }

    public bool IsCrouchButtonReleased()
    {
        return Input.GetButtonUp(controllerName + CROUCH);
    }

    public bool IsMovementActionButtonPressed()
    {
        return Input.GetButtonDown(controllerName + MOVEMENT_ACTION);
    }

    public bool IsMovementActionButtonHeld()
    {
        return Input.GetButton(controllerName + MOVEMENT_ACTION);
    }

    public bool IsMovementActionButtonReleased()
    {
        return Input.GetButtonUp(controllerName + MOVEMENT_ACTION);
    }

    public bool IsPrimaryAttackButtonPressed()
    {
        return Input.GetButtonDown(controllerName + PRIMARY_ATTACK);
    }

    public bool IsPrimaryAttackButtonHeld()
    {
        return Input.GetButton(controllerName + PRIMARY_ATTACK);
    }

    public bool IsPrimaryAttackButtonReleased()
    {
        return Input.GetButtonUp(controllerName + PRIMARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonPressed()
    {
        return Input.GetButtonDown(controllerName + SECONDARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonHeld()
    {
        return Input.GetButton(controllerName + SECONDARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonReleased()
    {
        return Input.GetButtonUp(controllerName + SECONDARY_ATTACK);
    }

    public bool IsNoMovementControlPressed()
    {
        return !Input.GetButton(controllerName + MOVE_LEFT) && !Input.GetButton(controllerName + MOVE_RIGHT);
    }

    public float GetMovement()
    { 
        return Input.GetAxis(controllerName + AXIS_HORIZONTAL);
    }

    //TODO: lock to 4 or 8 directions?
    /*
    public Vector2 GetAimDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        var heading = mousePos - transform.position;

        return heading / heading.magnitude;
    }
    */
}
