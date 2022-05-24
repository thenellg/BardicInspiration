using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterSetUp : MonoBehaviour
{
    public CharacterStats stats;


    private void Start()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void placeCharacter()
    {
        if(stats.activeTile == null)
        {
            OverlayTile tile = GetFocusedOnTile();

            //PositionCharacterOnTile(tile);
        }
    }

    public OverlayTile GetFocusedOnTile()
    {
        RaycastHit [] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 5f);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<OverlayTile>())
            {
                Debug.Log("I did it!");
                return hit.collider.GetComponent<OverlayTile>();
            }
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
}
