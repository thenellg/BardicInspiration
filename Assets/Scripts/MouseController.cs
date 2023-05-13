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
    public Pathfinder pathfinder;
    public RangeFinder rangeFinder;
    private DrawArrow drawArrow;

    public List<OverlayTile> path = new List<OverlayTile>();
    public List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    public List<OverlayTile> magicRangeTiles = new List<OverlayTile>();

    public bool isMoving = false;
    public bool activeMovement = true;
    private bool enemyMoving = false;

    public bool gameActive = true;
    public bool cursorActive = true;
    

    private OverlayTile cursorCurrentTile;
    private bool showingPuzzleSpots = false;

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
        if (gameActive && cursorActive)
        {
            RaycastHit2D hit = GetFocusedOnTile();

            if (hit)
            {
                OverlayTile tile = hit.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
                cursorCurrentTile = tile;

                if (inRangeTiles.Contains(tile) && !isMoving && activeMovement)
                {
                    if (battleManager.magicAttacking)
                    {
                        if (battleManager.currentSpell.spellType == Spell.spellTypes.AreaOfEffect)
                        {
                            //get range from tile into magicRangeTiles
                            if (inRangeTiles.Contains(tile))
                            {
                                GetInRangeMagicTiles(false, Vector2.zero);
                            }

                        }
                        else if (battleManager.currentSpell.spellType == Spell.spellTypes.Line)
                        {
                            //get range in line into magicRangeTiles
                            if (inRangeTiles.Contains(tile))
                            {
                                Vector2 direction = new Vector2(tile.gridLocation.x, tile.gridLocation.y) - new Vector2(character.activeTile.gridLocation.x, character.activeTile.gridLocation.y);
                                direction = new Vector2Int((int)direction.x, (int)direction.y);
                                direction = direction.normalized;


                                Debug.Log(direction);
                                GetInRangeMagicTiles(true, direction);
                            }
                            else
                            {
                                foreach (OverlayTile item in magicRangeTiles)
                                {
                                    item.HideTile();
                                    if (inRangeTiles.Contains(item))
                                        item.ShowTile();
                                }
                                magicRangeTiles.Clear();
                            }
                        }
                        else
                        {
                            if (inRangeTiles.Contains(tile))
                            {
                                magicRangeTiles.Clear();
                                magicRangeTiles.Add(tile);
                            }
                        }
                    }
                    else
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
                }
                else
                {
                    if (battleManager.magicAttacking)
                    {
                        foreach(OverlayTile item in magicRangeTiles)
                        {
                            if (inRangeTiles.Contains(item))
                            {
                                item.SetColor(item.settings.CanMoveHighlight);
                                item.ShowTile(true);
                            }
                            else
                                item.HideTile();
                        }
                        magicRangeTiles.Clear();
                    }
                }

                if (!battleManager.attacking && !battleManager.magicAttacking)
                {
                    if (tile.puzzleSpace == true)
                    {
                        foreach (GameObject ruin in battleManager.interactives)
                        {
                            if (ruin.GetComponent<Interactive>().activeTile == tile)
                            {
                                ruin.GetComponent<Interactive>().showTiles();
                            }
                            showingPuzzleSpots = true;
                        }
                    }
                    else
                    {
                        if (showingPuzzleSpots)
                        {
                            foreach (GameObject ruin in battleManager.interactives)
                            {
                                ruin.GetComponent<Interactive>().hideTiles();
                            }
                            showingPuzzleSpots = false;
                        }
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (battleManager.attacking && inRangeTiles.Contains(tile))
                    {
                        //charcter attacks
                        battleManager.attack(character, tile.currentChar);

                        battleManager.attacking = false;
                        isMoving = true;
                        cursorActive = false;
                    }
                    else if (battleManager.magicAttacking && inRangeTiles.Contains(tile))
                    {
                        battleManager.magicAttack(magicRangeTiles);

                        battleManager.attacking = false;
                        battleManager.magicAttacking = false;
                        isMoving = true;
                        cursorActive = false;
                        path.Clear();

                        magicRangeTiles.Clear();
                    }
                    else if (inRangeTiles.Contains(tile) && (!tile.isBlocked || tile.currentChar == character))
                    {
                        isMoving = true;
                        tile.gameObject.GetComponent<OverlayTile>().HideTile();

                        if (tile.puzzleSpace == true)
                        {
                            foreach (GameObject ruin in battleManager.interactives)
                            {
                                if (ruin.GetComponent<Interactive>().activeTile == tile)
                                {
                                    ruin.GetComponent<Interactive>().showTiles();
                                    break;
                                }
                            }
                        }

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

                if (character.activeTile.puzzleSpace == true)
                {
                    foreach (GameObject ruin in battleManager.interactives)
                    {
                        if (ruin.GetComponent<Interactive>().activeTile == character.activeTile)
                        {
                            ruin.GetComponent<Interactive>().showTiles();
                            break;
                        }
                    }
                }

                //Set to turn abilities in battle manager if player
                //Run enemy idea if an enemy
                if (character.tag == "Player Team")// && !battleManager.attacking)
                {
                    activeMovement = false;
                    battleManager.actionMenu.setActionMenuLocation(character);
                    battleManager.actionMenu.visibleActionMenu.SetActive(true);
                    battleManager.actionMenu.setActionMenu();
                    cursorActive = false;
                }
                else if (character.tag == "Enemy Team")
                {
                    OverlayTile defender = character.GetComponent<BasicEnemy>().enemyAttackCheck();
                    if (defender)
                    {
                        Debug.Log("Player is in range");
                        //Eventually adjust this for possible special attacks?
                        battleManager.onRuin = false;
                        battleManager.damageAmount = character.attack;
                        battleManager.attack(character, defender.currentChar);
                    }
                }

            }
        }
    }

    public void GetInRangeTiles(bool overrideChar = false)
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        if (overrideChar)
            inRangeTiles = rangeFinder.GetTilesinRange(character.activeTile, 100);
        else
            inRangeTiles = rangeFinder.GetTilesinRange(character.activeTile, character.speed);

        if (character.tag == "Player Team" || character.tag == "Enemy Team")
        {
            foreach (var item in inRangeTiles)
            {
                item.ShowTile();
            }
            character.activeTile.ShowTile(true);
        }
    }

    public void GetInRangeMagicTiles(bool line, Vector2 direction)
    {
        foreach (OverlayTile item in magicRangeTiles)
        {
            if (inRangeTiles.Contains(item))
            {
                item.SetColor(item.settings.CanMoveHighlight);
                item.ShowTile(true);
            }
            else
                item.HideTile();
        }

        if (line)
        {
            List<OverlayTile> temp = new List<OverlayTile>();
            Vector2 baseTile = character.activeTile.gridLocation2D;
            foreach (OverlayTile tile in inRangeTiles)
            {
                if (direction.x == 0 && direction.y > 0 && (tile.gridLocation2D.x <= (baseTile.x + battleManager.currentSpell.minSpellRange / 2)) && (tile.gridLocation2D.x >= (baseTile.x - battleManager.currentSpell.minSpellRange / 2)) && tile.gridLocation2D.y > baseTile.y)
                    temp.Add(tile);
                else if (direction.x == 0 && direction.y < 0 && (tile.gridLocation2D.x <= (baseTile.x + battleManager.currentSpell.minSpellRange / 2)) && (tile.gridLocation2D.x >= (baseTile.x - battleManager.currentSpell.minSpellRange / 2)) && tile.gridLocation2D.y < baseTile.y)
                    temp.Add(tile);
                else if (direction.x > 0 && direction.y == 0 && (tile.gridLocation2D.y <= (baseTile.y + battleManager.currentSpell.minSpellRange / 2)) && (tile.gridLocation2D.y >= (baseTile.y - battleManager.currentSpell.minSpellRange / 2)) && tile.gridLocation2D.x > baseTile.x)
                    temp.Add(tile);
                else if (direction.x < 0 && direction.y == 0 && (tile.gridLocation2D.y <= (baseTile.y + battleManager.currentSpell.minSpellRange / 2)) && (tile.gridLocation2D.y >= (baseTile.y - battleManager.currentSpell.minSpellRange / 2)) && tile.gridLocation2D.x < baseTile.x)
                    temp.Add(tile);

                if (temp != null)
                    magicRangeTiles = temp;

                temp.Clear();
            }
        }
        else
        {
            magicRangeTiles = rangeFinder.GetTilesinRange(cursorCurrentTile, battleManager.currentSpell.minSpellRange);
        }

        foreach (var item in magicRangeTiles)
        {
            item.SetColor(battleManager.settings.targetHighlight);
            item.ShowTile(true);
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

        if (Vector2.Distance(character.transform.position, path[0].characterPos.position) < 0.0001f)
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

        if (character.tag == "Player Team")
            battleManager.playerLocations.Remove(character.activeTile);

        character.activeTile = overlayTile;

        if (character.tag == "Player Team")
            battleManager.playerLocations.Add(character.activeTile);

        overlayTile.currentChar = character;
    }

    public void enemyMove()
    {
        //Go through battleManager.playerLocations to find the closest player
        int checkA = pathfinder.GetGridDistance(character.activeTile, battleManager.playerLocations[0]);
        OverlayTile nearestCharacter = battleManager.playerLocations[0];

        foreach (OverlayTile newTile in battleManager.playerLocations)
        {
            int testA = pathfinder.GetGridDistance(character.activeTile, newTile);
            if (testA < checkA)
            {
                checkA = testA;

                //if both are in range, check which is the weaker enemy or if the enemy has a type advantage

                nearestCharacter = newTile;
            }
        }

        //move as close to them as possible
        OverlayTile closestTile = inRangeTiles[0];
        int checkB = pathfinder.GetGridDistance(nearestCharacter, inRangeTiles[0]);

        foreach (OverlayTile closest in inRangeTiles)
        {
            int testB = pathfinder.GetGridDistance(nearestCharacter, closest);
            if (testB < checkB && testB != 0)
            {
                Debug.Log(testB + " vs " + checkB);
                checkB = testB;
                closestTile = closest;
            }
        }
        path = pathfinder.FindPath(character.activeTile, closestTile, inRangeTiles);
        isMoving = true;

    }

    public void enemyAttackCheck()
    {
        List<OverlayTile> neighborTiles = MapManager.Instance.GetNeighborTiles(character.activeTile, inRangeTiles);

    }
}
