using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile highlightTilePrefab;
    public GameObject highlightContainer;

    public Dictionary<Vector2, OverlayTile> colliderMap;

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
        colliderMap = new Dictionary<Vector2, OverlayTile>();
        foreach (var map in gameObject.GetComponentsInChildren<Tilemap>())
        {
            if (map.tag != "Level - No Grid")
            {
                BoundsInt bounds = map.cellBounds;

                //looping through all tiles in each layer
                for (int z = bounds.max.z; z >= bounds.min.z; z--)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (int x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            var tileLocation = new Vector3Int(x, y, z);
                            Vector2 tileKey = new Vector2(x, y);

                            if (map.HasTile(tileLocation) && !colliderMap.ContainsKey(tileKey))
                            {
                                var highlightTile = Instantiate(highlightTilePrefab, highlightContainer.transform);
                                var cellWorldPosition = map.GetCellCenterWorld(tileLocation);


                                highlightTile.transform.position = new Vector3(cellWorldPosition.x + 0.1604f, cellWorldPosition.y + 0.1599f, cellWorldPosition.z);
                                highlightTile.GetComponent<SpriteRenderer>().sortingOrder = map.GetComponent<TilemapRenderer>().sortingOrder;
                                highlightTile.gridLocation = tileLocation;
                                colliderMap.Add(tileKey, highlightTile);
                            }
                        }
                    }
                }
            }
        }
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
            if(Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) < 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Bottom
        locationToCheck = new Vector2(currentTile.gridLocation.x, currentTile.gridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) < 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Right
        locationToCheck = new Vector2(currentTile.gridLocation.x + 1, currentTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) < 1)
                neighbors.Add(map[locationToCheck]);
        }

        //Left
        locationToCheck = new Vector2(currentTile.gridLocation.x - 1, currentTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentTile.gridLocation.z - map[locationToCheck].gridLocation.z) < 1)
                neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }

}
