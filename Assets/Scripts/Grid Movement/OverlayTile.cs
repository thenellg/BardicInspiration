using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;

public class OverlayTile : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform characterPos;
    public SpriteRenderer arrowSprite;

    //Settting up Overlay Tiles as nodes for pathfinding
    public int g;
    public int h;
    public int f { get { return g + h; } }

    public bool isBlocked;
    public OverlayTile previous;
    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public List<Sprite> arrows = new List<Sprite>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);    //Edit this line to account for settings later
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void setArrowSprite(ArrowDirections direction)
    {
        if (direction == ArrowDirections.None)
        {
            arrowSprite.color = new Color(arrowSprite.color.r, arrowSprite.color.g, arrowSprite.color.b, 0);
        }
        else
        {
            arrowSprite.color = new Color(arrowSprite.color.r, arrowSprite.color.g, arrowSprite.color.b, 1); //Edit this line to account for settings later
            arrowSprite.sprite = arrows[(int)direction];
            arrowSprite.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}