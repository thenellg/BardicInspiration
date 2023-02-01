using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveSetUp : MonoBehaviour
{
    public Interactive interactive;
    public Vector2 startingLocation = new Vector2();

    private void Start()
    {
        //stats = GetComponent<CharacterStats>();
    }

    public void placeInteractive(Dictionary<Vector2, OverlayTile> colliderMap)
    {
        if (interactive.activeTile == null)
        {
            OverlayTile tile = GetFocusedOnTile(colliderMap);
            Debug.Log(tile.gameObject);
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
