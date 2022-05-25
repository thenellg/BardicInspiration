using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile highlightTilePrefab;
    public GameObject highlightContainer;

    public GameObject playerTeam;
    public GameObject enemyTeam;

    public Dictionary<Vector2, OverlayTile> colliderMap;

    public BattleManager battleManager;
    public Color settingsColor = new Color(1, 1, 1, 1);

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get Settings and update information


        int numLayer = 0;
        //var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>();

        colliderMap = new Dictionary<Vector2, OverlayTile>();

        foreach (var map in tileMaps)
        {
            if (map.tag != "Level - No Grid")
            { 
            
                numLayer = map.GetComponent<TilemapRenderer>().sortingOrder;
                BoundsInt bounds = map.cellBounds;

                //looping through all tiles in each layer
                for (int z = bounds.max.z; z >= bounds.min.z; z--)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (int x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            var tileLocation = new Vector3Int(x, y, z);
                            Vector2Int tileKey = new Vector2Int(x, y);

                            if (map.HasTile(tileLocation) && !colliderMap.ContainsKey(tileKey))
                            {
                                var highlightTile = Instantiate(highlightTilePrefab, highlightContainer.transform);
                                var cellWorldPosition = map.GetCellCenterWorld(tileLocation);

                                //Commented out version includes adjustments for using Tiled.
                                //highlightTile.transform.position = new Vector3(cellWorldPosition.x + 0.1604f, cellWorldPosition.y + 0.1599f, cellWorldPosition.z);
                                
                                highlightTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                                highlightTile.GetComponent<SpriteRenderer>().sortingOrder = map.GetComponent<TilemapRenderer>().sortingOrder;
                                highlightTile.gridLocation = tileLocation;

                                //highlightTile.SetColor(settingsColor);

                                if (map.tag == "Level - Half Step")
                                {
                                    highlightTile.isHalfTile = true;
                                    highlightTile.transform.position = new Vector3(highlightTile.transform.position.x, highlightTile.transform.position.y - 0.0799f, 0);
                                }
                                else
                                {
                                    highlightTile.transform.position = new Vector3(highlightTile.transform.position.x, highlightTile.transform.position.y, 0);
                                }

                                colliderMap.Add(tileKey, highlightTile);
                            }
                        }

                    }
                }
            }
        }

        //Set Up Characters
        foreach(var hero in playerTeam.GetComponentsInChildren<CharacterSetUp>())
        {
            hero.placeCharacter(colliderMap);
            FindObjectOfType<MouseController>().character = hero.GetComponent<CharacterStats>();
            battleManager.playerTeam.Add(hero.gameObject);

            battleManager.turnOrder.Add(hero.gameObject);
        }

        //Set Up Enemies
        foreach (var enemy in enemyTeam.GetComponentsInChildren<CharacterSetUp>())
        {
            enemy.placeCharacter(colliderMap);
            battleManager.enemyTeam.Add(enemy.gameObject);

            battleManager.turnOrder.Add(enemy.gameObject);
        }

        //Set Up Cursor
        List<GameObject> sortedList = battleManager.turnOrder.OrderByDescending(o => o.GetComponent<CharacterStats>().speed).ToList();
        battleManager.turnOrder = sortedList;

        FindObjectOfType<MouseController>().character = battleManager.turnOrder[0].GetComponent<CharacterStats>();
    }

    int getSpot(CharacterStats hero)
    {
        for (int i = 0; i < battleManager.turnOrder.Count; i++)
        {
            if (battleManager.turnOrder[i].GetComponent<CharacterStats>().speed < hero.speed)
            {
                return i;
            }
        }
        return 0;
    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile currentTile, List<OverlayTile> searchableTiles)
    {
        Dictionary<Vector2, OverlayTile> map = new Dictionary<Vector2, OverlayTile>();
        if(searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                map.Add(item.gridLocation2D, item);
            }
        }
        else
        {
            map = colliderMap;
        }

        List<OverlayTile> neighbors = new List<OverlayTile>();

        Vector2 locationToCheck;

        //Top
        locationToCheck = new Vector2(currentTile.gridLocation.x, currentTile.gridLocation.y + 1);
        if (map.ContainsKey(locationToCheck)) 
        {
            if(Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Bottom
        locationToCheck = new Vector2(currentTile.gridLocation.x, currentTile.gridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Right
        locationToCheck = new Vector2(currentTile.gridLocation.x + 1, currentTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Left
        locationToCheck = new Vector2(currentTile.gridLocation.x - 1, currentTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }

}
