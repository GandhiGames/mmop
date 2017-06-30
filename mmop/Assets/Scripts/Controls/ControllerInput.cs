using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: remove duplicaton in this and keyboard input.
public class ControllerInput : MonoBehaviour, PlayerControls
{
    public string controllerName;

    private static readonly string JUMP = " Jump";
    private static readonly string MOVEMENT_ACTION = " Movement Action";
    private static readonly string PRIMARY_ATTACK = " Primary Attack";
    private static readonly string SECONDARY_ATTACK = " Secondary Attack";
    private static readonly string AXIS_HORIZONTAL = " Horizontal";
    private static readonly string AXIS_VERTICAL = " Vertical";
    private static readonly string AXIS_HORIZONTAL_DPAD = " Horizontal Dpad";
    private static readonly string AXIS_VERTICAL_DPAD = " Vertical Dpad";

    private float previousCrouchStatusDpad = 0f;
    private float previousCrouchStatusStick = 0f;

    void LateUpdate()
    {
        previousCrouchStatusStick = Input.GetAxisRaw(controllerName + AXIS_VERTICAL);
        previousCrouchStatusDpad = Input.GetAxisRaw(controllerName + AXIS_VERTICAL_DPAD);
    }

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

    //TODO: Test this works - double tapping to drop from platform with controller.
    public bool IsCrouchButtonPressed()
    {
        return (Input.GetAxisRaw(controllerName + AXIS_VERTICAL) < 0f && previousCrouchStatusStick >= 0f)
            || (Input.GetAxisRaw(controllerName + AXIS_VERTICAL_DPAD) < 0f && previousCrouchStatusDpad >= 0f);
    }

    public bool IsCrouchButtonHeld()
    {
        return Input.GetAxisRaw(controllerName + AXIS_VERTICAL) < 0f ||
            Input.GetAxisRaw(controllerName + AXIS_VERTICAL_DPAD) < 0f;
    }

    public bool IsCrouchButtonReleased()
    {
        return (Input.GetAxisRaw(controllerName + AXIS_VERTICAL) >= 0f && previousCrouchStatusStick < 0f)
            || (Input.GetAxisRaw(controllerName + AXIS_VERTICAL_DPAD) >= 0f && previousCrouchStatusDpad < 0f);
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
        return GetMovement() == 0f;
    }

    public float GetMovement()
    {
        float joyAxis = Input.GetAxis(controllerName + AXIS_HORIZONTAL);

        if(joyAxis != 0f)
        {
            return joyAxis;
        }

        return Input.GetAxis(controllerName + AXIS_HORIZONTAL_DPAD);
    }
}
