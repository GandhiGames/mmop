using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    Left,
    Right
}

public interface PlayerDirection
{
    FacingDirection currentDirection { get; }
    void Face(FacingDirection direction);
}

public class PlayerFacingDirection : MonoBehaviour, PlayerDirection
{
    public FacingDirection direction = FacingDirection.Right;
    public FacingDirection currentDirection { get { return direction; } }

    public void Face(FacingDirection direction)
    {
        if(this.direction == direction)
        {
            return;
        }

        Flip();
    }

    private void Flip()
    {
        if (direction == FacingDirection.Left)
        {
            direction = FacingDirection.Right;
        }
        else
        {
            direction = FacingDirection.Left;
        }
        
        Vector3 curScale = transform.localScale;
        curScale.x *= -1;
        transform.localScale = curScale;
    }
}
