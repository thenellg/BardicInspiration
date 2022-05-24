using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterSetUp : MonoBehaviour
{
    public CharacterStats stats;
    public Vector2 startingLocation = new Vector3();

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void placeCharacter(Dictionary<Vector2, OverlayTile> colliderMap)
    {
        if(stats.activeTile == null)
        {
            OverlayTile tile = GetFocusedOnTile(colliderMap);
            Debug.Log(tile.gameObject);
            PositionCharacterOnTile(tile);
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

    private void PositionCharacterOnTile(OverlayTile overlayTile)
    {
        this.transform.position = overlayTile.characterPos.position;
        stats.activeTile = overlayTile;
        overlayTile.isBlocked = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");
    }
}
