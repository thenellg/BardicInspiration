using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static DrawArrow;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float moveSpeed;

    public GameObject characterPrefab;
    [SerializeField] private CharacterStats character;

    [SerializeField] private OverlayTile overlayTile = null;
    private Pathfinder pathfinder;
    private RangeFinder rangeFinder;
    private DrawArrow drawArrow;

    public List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    public bool isMoving = false;

    private void Start()
    {
        pathfinder = new Pathfinder();
        rangeFinder = new RangeFinder();
        drawArrow = new DrawArrow();

        path = new List<OverlayTile>();
        isMoving = false;
        inRangeTiles = new List<OverlayTile>();
    }

    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (inRangeTiles.Contains(tile) && !isMoving && character != null)
            {
                path = pathfinder.FindPath(character.activeTile, tile, inRangeTiles);

                foreach (var item in inRangeTiles)
                {
                    item.setArrowSprite(ArrowDirections.None);
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
                tile.ShowTile();

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterStats>();
                    PositionCharacterOnLine(tile);
                    GetInRangeTiles();
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
        else if (path.Count == 0 && character != null)
        {
            GetInRangeTiles();
            isMoving = false;
        }
    }

    private void MoveAlongPath()
    {
        var step = moveSpeed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].characterPos.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].characterPos.position) < 0.0001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        character.transform.position = tile.characterPos.position;
        character.activeTile = tile;
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void GetInRangeTiles()
    {
        inRangeTiles = rangeFinder.GetTilesinRange(character.activeTile, character.speed);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }
}
