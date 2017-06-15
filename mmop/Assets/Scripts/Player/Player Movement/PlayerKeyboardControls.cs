using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardControls : MonoBehaviour, PlayerControls
{
    public bool IsJumpButtonPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool IsJumpButtonHeld()
    {
        return Input.GetButton("Jump");
    }

    public bool IsJumpButtonReleased()
    {
        return Input.GetButtonUp("Jump");
    }

    public bool IsCrouchButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.S);
    }

    public bool IsCrouchButtonHeld()
    {
        return Input.GetKey(KeyCode.S);
    }

    public bool IsCrouchButtonReleased()
    {
        return Input.GetKeyUp(KeyCode.S);
    }

    public bool IsMovementActionButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public bool IsMovementActionButtonHeld()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool IsMovementActionButtonReleased()
    {
        return Input.GetKeyUp(KeyCode.LeftShift);
    }

    public bool IsPrimaryAttackButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool IsPrimaryAttackButtonHeld()
    {
        return Input.GetKey(KeyCode.E);
    }

    public bool IsPrimaryAttackButtonReleased()
    {
        return Input.GetKeyUp(KeyCode.E);
    }

    public bool IsSecondaryAttackButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public bool IsSecondaryAttackButtonHeld()
    {
        return Input.GetKey(KeyCode.Q);
    }

    public bool IsSecondaryAttackButtonReleased()
    {
        return Input.GetKeyUp(KeyCode.Q);
    }

    public float GetMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    public Vector2 GetAimDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        var heading = mousePos - transform.position;

        return heading / heading.magnitude;
    }
}
