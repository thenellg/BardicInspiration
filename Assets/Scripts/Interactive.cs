using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public GameObject minigame;
    public OverlayTile activeTile = null;
    public OverlayTile[] inRangeTiles;
    public BattleManager battleManager;
    public MouseController cursor;

    //[Header("Reaction")]
    public enum reactionType
    {
        damage, terrain, heal
    }
    public reactionType reaction;
    public GameObject gate;
    public int damageOrHealInt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void damageEnemies()
    {
        List<CharacterStats> characters = new List<CharacterStats>();
        foreach(OverlayTile tile in inRangeTiles)
        {
            //tile animation
            if (tile.currentChar != null)
            {
                battleManager.attack(null, tile.currentChar);
                battleManager.doDamage(cursor.character, damageOrHealInt);
            }
        }
    }


    public void heal()
    {

    }
}
