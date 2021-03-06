using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float moveSpeed;

    public BattleManager battleManager;

    public GameObject characterPrefab;
    public CharacterStats character;

    [SerializeField] private OverlayTile overlayTile = null;
    private Pathfinder pathfinder;
    private RangeFinder rangeFinder;
    private DrawArrow drawArrow;

    public List<OverlayTile> path = new List<OverlayTile>();
    public List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    public bool isMoving = false;
    public bool activeMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder();
        rangeFinder = new RangeFinder();
        drawArrow = new DrawArrow();
    }

    void beginRound()
    {
        battleManager.onTurnSwap();
        GetInRangeTiles();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = GetFocusedOnTile();

        if (hit)
        {
            OverlayTile tile = hit.collider.gameObject.GetComponent<OverlayTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (inRangeTiles.Contains(tile) && !isMoving && activeMovement)
            {
                path = pathfinder.FindPath(character.activeTile, tile, inRangeTiles);

                foreach (var item in inRangeTiles)
                {
                    MapManager.Instance.colliderMap[item.gridLocation2D].setArrowSprite(ArrowDirections.None);
                }

                for (int i = 0; i < path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i - 1] : character.activeTile;
                    var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                    var arrow = drawArrow.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].setArrowSprite(arrow);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (battleManager.attacking)
                {
                    //charcter attacks
                    battleManager.attack(character, tile.currentChar);

                    battleManager.attacking = false;
                    isMoving = true;
                }
                else
                {
                    isMoving = true;
                    tile.gameObject.GetComponent<OverlayTile>().HideTile();
                }
            }
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }
        else if (path.Count == 0 && isMoving)
        {
            isMoving = false;

            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }

            if (character.tag == "Player Team")
            {
                activeMovement = false;
                battleManager.actionMenu.setActionMenuLocation(character);
                battleManager.actionMenu.visibleActionMenu.SetActive(true);
            }

            //Set to turn abilities in battle manager if player
            //Run enemy idea if an enemy
        }


    }

    public void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        inRangeTiles = rangeFinder.GetTilesinRange(character.activeTile, character.speed);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
        character.activeTile.ShowTile(true);
    }

    void MoveAlongPath()
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        var step = moveSpeed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].characterPos.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if(Vector2.Distance(character.transform.position, path[0].characterPos.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    private RaycastHit2D GetFocusedOnTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hits = Physics2D.Raycast(mousePos, Vector2.zero);
        Debug.DrawRay(mousePos, Vector2.zero, Color.red);

        return hits;
    }

    private void PositionCharacterOnTile(OverlayTile overlayTile)
    {
        character.activeTile.isBlocked = false;
        character.activeTile.currentChar = null;

        character.transform.position = overlayTile.GetComponent<OverlayTile>().characterPos.position;

        overlayTile.isBlocked = true;        

        character.activeTile = overlayTile;
        overlayTile.currentChar = character;
    }
}
