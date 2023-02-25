using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSetUp : MonoBehaviour
{
    public Interactive interactive;
    public Vector2 startingLocation = new Vector2();
    public List<Vector2> damageLocations = new List<Vector2>();

    private void Start()
    {
        //stats = GetComponent<CharacterStats>();
    }

    public void placeInteractive(Dictionary<Vector2, OverlayTile> colliderMap)
    {
        if (interactive.activeTile == null)
        {
            OverlayTile tile = GetFocusedOnTile(colliderMap);
            GetDamageTiles(colliderMap);
            //Debug.Log(tile.gameObject);
            PositionInteractiveOnTile(tile);
            interactive.activeTile = tile;
        }
    }

    public OverlayTile GetFocusedOnTile(Dictionary<Vector2, OverlayTile> colliderMap)
    {
        if (colliderMap.ContainsKey(startingLocation))
        {
            return colliderMap[startingLocation];
        }

        Debug.Log("ERROR: No Tile Found");
        return null;
    }

    public void GetDamageTiles(Dictionary<Vector2, OverlayTile> colliderMap)
    {
        List<OverlayTile> damageTiles = new List<OverlayTile>();
        foreach(Vector2 location in damageLocations)
        {
            //Debug.Log(location);
            if (colliderMap.ContainsKey(location))
            {
                //Debug.Log("Ruin Tile Found");
                damageTiles.Add(colliderMap[location]);
            }
        }

        this.GetComponent<Interactive>().inRangeTiles = damageTiles;
    }

    private void PositionInteractiveOnTile(OverlayTile overlayTile)
    {
        this.transform.position = overlayTile.characterPos.position;
        overlayTile.puzzleSpace = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");
    }
}
