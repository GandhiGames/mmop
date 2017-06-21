using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour, PlayerControls
{
    private static readonly string CONTROLLER_NAME = "Keyboard";

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
        return Input.GetButtonDown(CONTROLLER_NAME + JUMP);
    }

    public bool IsJumpButtonHeld()
    {
        return Input.GetButton(CONTROLLER_NAME + JUMP);
    }

    public bool IsJumpButtonReleased()
    {
        return Input.GetButtonUp(CONTROLLER_NAME + JUMP);
    }

    public bool IsCrouchButtonPressed()
    {
        return Input.GetButtonDown(CONTROLLER_NAME + CROUCH);
    }

    public bool IsCrouchButtonHeld()
    {
        return Input.GetButton(CONTROLLER_NAME + CROUCH);
    }

    public bool IsCrouchButtonReleased()
    {
        return Input.GetButtonUp(CONTROLLER_NAME + CROUCH);
    }

    public bool IsMovementActionButtonPressed()
    {
        return Input.GetButtonDown(CONTROLLER_NAME + MOVEMENT_ACTION);
    }

    public bool IsMovementActionButtonHeld()
    {
        return Input.GetButton(CONTROLLER_NAME + MOVEMENT_ACTION);
    }

    public bool IsMovementActionButtonReleased()
    {
        return Input.GetButtonUp(CONTROLLER_NAME + MOVEMENT_ACTION);
    }

    public bool IsPrimaryAttackButtonPressed()
    {
        return Input.GetButtonDown(CONTROLLER_NAME + PRIMARY_ATTACK);
    }

    public bool IsPrimaryAttackButtonHeld()
    {
        return Input.GetButton(CONTROLLER_NAME + PRIMARY_ATTACK);
    }

    public bool IsPrimaryAttackButtonReleased()
    {
        return Input.GetButtonUp(CONTROLLER_NAME + PRIMARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonPressed()
    {
        return Input.GetButtonDown(CONTROLLER_NAME + SECONDARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonHeld()
    {
        return Input.GetButton(CONTROLLER_NAME + SECONDARY_ATTACK);
    }

    public bool IsSecondaryAttackButtonReleased()
    {
        return Input.GetButtonUp(CONTROLLER_NAME + SECONDARY_ATTACK);
    }

    public bool IsNoMovementControlPressed()
    {
        return !Input.GetButton(CONTROLLER_NAME + MOVE_LEFT) && !Input.GetButton(CONTROLLER_NAME + MOVE_RIGHT);
    }

    public float GetMovement()
    { 
        return Input.GetAxis(CONTROLLER_NAME + AXIS_HORIZONTAL);
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
