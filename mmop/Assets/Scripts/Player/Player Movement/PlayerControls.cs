using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerControls
{
    bool IsJumpButtonPressed();
    bool IsJumpButtonHeld();
    bool IsJumpButtonReleased();

    bool IsCrouchButtonPressed();
    bool IsCrouchButtonHeld();
    bool IsCrouchButtonReleased();

    bool IsMovementActionButtonPressed();
    bool IsMovementActionButtonHeld();
    bool IsMovementActionButtonReleased();

    bool IsPrimaryAttackButtonPressed();
    bool IsPrimaryAttackButtonHeld();
    bool IsPrimaryAttackButtonReleased();

    bool IsSecondaryAttackButtonPressed();
    bool IsSecondaryAttackButtonHeld();
    bool IsSecondaryAttackButtonReleased();

    float GetMovement();

    Vector2 GetAimDirection();
}
