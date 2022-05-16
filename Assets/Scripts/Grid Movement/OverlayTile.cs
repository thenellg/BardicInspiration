using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;

public class OverlayTile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform characterPos;
    public SpriteRenderer arrowImage;

    //Settting up Overlay Tiles as nodes for pathfinding
    public int g;
    public int h;
    public int f{ get { return g + h; } }
    
    public bool isBlocked;
    public OverlayTile previous;
    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y);  } }

    public bool isHalfTile = false;
    public List<Sprite> arrows = new List<Sprite>();

    public void ShowTile()
    {
        setArrowSprite(ArrowDirections.None);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);    //Edit this line to account for settings later
    }

    public void HideTile()
    {
        setArrowSprite(ArrowDirections.None);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void setArrowSprite(ArrowDirections direction)
    {
        if (direction == ArrowDirections.None)
        {
            arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 0);
        }
        else
        {
            arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 1); //Edit this line to account for settings later
            arrowImage.sprite = arrows[(int)direction];
            arrowImage.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
