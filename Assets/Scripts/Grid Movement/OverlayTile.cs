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
    public CharacterStats currentChar;

    public OverlayTile previous;
    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y);  } }

    public bool isHalfTile = false;
    public List<Sprite> arrows = new List<Sprite>();
    public GameSettings settings { get { return FindObjectOfType<GameSettings>(); } }
    public bool puzzleSpace = false;

    public void SetColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
    }

    public void ShowTile(bool attacking = false)
    {
        if (!isBlocked)
        {
            setArrowSprite(ArrowDirections.None);
            Color color = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
            //gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
        }
        else if (attacking)
        {
            setArrowSprite(ArrowDirections.None);
            Color color = gameObject.GetComponent<SpriteRenderer>().color;


            if (currentChar && currentChar.tag == "Enemy Team")
                gameObject.GetComponent<SpriteRenderer>().color = settings.targetHighlight;
            else if (currentChar && currentChar.tag == "Player Team")
                gameObject.GetComponent<SpriteRenderer>().color = settings.TeammateHighlight;
            else
                gameObject.GetComponent<SpriteRenderer>().color = settings.CanMoveHighlight;
        }

        // Add and else here that changes colors based on what is blocking (if player, playerBlock, if enemy enemyBlock)
    }

    public void showRuinTile()
    {
        setArrowSprite(ArrowDirections.None);
        Color color = settings.CanMoveHighlight;
        if (!isBlocked)
        {
            color = settings.ruinHighlight;
        }
        else
        {
            if (currentChar && currentChar.tag == "Enemy Team")
                color = settings.targetHighlight;
            if (currentChar && currentChar.tag == "Player Team")
                color = settings.TeammateHighlight;
        }


        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);

    }

    public void HideTile()
    {
        setArrowSprite(ArrowDirections.None);
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(settings.CanMoveHighlight.r, settings.CanMoveHighlight.g, settings.CanMoveHighlight.b, 0);
    }

    public void setArrowSprite(ArrowDirections direction)
    {
        if (direction == ArrowDirections.None)
        {
            arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, 0);
        }
        else
        {
            arrowImage.color = settings.directionArrow; //Edit this line to account for settings later
            arrowImage.sprite = arrows[(int)direction];
            arrowImage.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
