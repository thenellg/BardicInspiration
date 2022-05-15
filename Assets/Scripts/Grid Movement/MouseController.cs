using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float moveSpeed;

    public GameObject characterPrefab;
    private CharacterStats character;

    [SerializeField] private GameObject overlayTile = null;
    private Pathfinder pathfinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder();
        rangeFinder = new RangeFinder();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = GetFocusedOnTile();

        if (hit)
        {
            GameObject tempOverlayTile = hit.collider.gameObject;
            cursor.transform.position = new Vector3(tempOverlayTile.transform.position.x, tempOverlayTile.transform.position.y, -1);
            cursor.GetComponent<SpriteRenderer>().sortingOrder = tempOverlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile = tempOverlayTile;

                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterStats>();
                    PositionCharacterOnTile(overlayTile.GetComponent<OverlayTile>());
                    GetInRangeTiles();
                }
                else
                {
                    path = pathfinder.FindPath(character.activeTile, overlayTile.GetComponent<OverlayTile>(), inRangeTiles);
                    Debug.Log("Path Length: " + path.Count());
                }
            }

        }
        if(path.Count > 0)
        {
            MoveAlongPath();
        }
    }
    
    private void GetInRangeTiles()
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

        if (path.Count == 0)
        {
            GetInRangeTiles();
        }
    }

    public RaycastHit2D GetFocusedOnTile()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hits = Physics2D.Raycast(mousePos, Vector2.zero);
        Debug.DrawRay(mousePos, Vector2.zero, Color.red);

        return hits;
    }

    private void PositionCharacterOnTile(OverlayTile overlayTile)
    {
        character.transform.position = overlayTile.GetComponent<OverlayTile>().characterPos.position;
        character.GetComponent<CharacterStats>().activeTile = overlayTile;
    }
}
