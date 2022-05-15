using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow
{
    public enum ArrowDirections 
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopLeft = 5,
        BottomLeft = 6,
        TopRight = 7,
        BottomRight = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }

    public ArrowDirections TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? currentTile.gridLocation2D - previousTile.gridLocation2D : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null ? futureTile.gridLocation2D - currentTile.gridLocation2D : new Vector2Int(0, 0);
        Vector2Int actualDirection = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if(previousTile == currentTile)
        {
            return ArrowDirections.None;
        }

        if (actualDirection == new Vector2Int(0, 1) && !isFinal)
        {
            return ArrowDirections.Up;
        }
        else if (actualDirection == new Vector2Int(0, -1) && !isFinal)
        {
            return ArrowDirections.Down;
        }
        else if (actualDirection == new Vector2Int(1, 0) && !isFinal)
        {
            return ArrowDirections.Left;
        }
        else if (actualDirection == new Vector2Int(-1, 0) && !isFinal)
        {
            return ArrowDirections.Right;
        }
        else if (actualDirection == new Vector2(1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirections.BottomLeft;
            else
                return ArrowDirections.TopRight;
        }

        else if (actualDirection == new Vector2(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirections.BottomRight;
            else
                return ArrowDirections.TopLeft;
        }

        else if (actualDirection == new Vector2(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirections.TopLeft;
            else
                return ArrowDirections.BottomRight;
        }

        else if (actualDirection == new Vector2(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirections.TopRight;
            else
                return ArrowDirections.BottomLeft;
        }
        else if(actualDirection == new Vector2Int(0, 1) && isFinal)
        {
            return ArrowDirections.UpFinished;
        }
        else if (actualDirection == new Vector2Int(0, -1) && isFinal)
        {
            return ArrowDirections.DownFinished;
        }
        else if (actualDirection == new Vector2Int(1, 0) && isFinal)
        {
            return ArrowDirections.LeftFinished;
        }
        else if (actualDirection == new Vector2Int(-1, 0) && isFinal)
        {
            return ArrowDirections.RightFinished;
        }

        return ArrowDirections.None;
    }

}
