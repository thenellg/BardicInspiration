using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;

public class OverlayTile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform characterPos;

    //Settting up Overlay Tiles as nodes for pathfinding
    public int g;
    public int h;
    public int f{ get { return g + h; } }
    
    public bool isBlocked;
    public OverlayTile previous;
    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y);  } }

    public List<Sprite> arrows = new List<Sprite>();


    void Start()
    {
        characterPos = GetComponentInChildren<Transform>();


    }

    public void ShowTile()
    {
        setArrowSprite(ArrowDirections.None);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);    //Edit this line to account for settings later
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void setArrowSprite(ArrowDirections direction)
    {
        SpriteRenderer arrow = GetComponentsInChildren<SpriteRenderer>()[0];
        if (direction == ArrowDirections.None)
        {
            arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 0);
        }
        else
        {
            arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 1); //Edit this line to account for settings later
            arrow.sprite = arrows[(int)direction];
            arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
