using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public GameObject minigame;
    public OverlayTile activeTile = null;
    public List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    public BattleManager battleManager;
    public MouseController cursor;

    //[Header("Reaction")]
    public enum reactionType
    {
        damage, terrain, heal
    }
    public reactionType reaction;
    public bool gate = true;
    public int damageOrHealInt;
    public GameObject playerTeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interaction()
    {
        if(reaction == reactionType.damage)
        {
            damageEnemies();
        }
        else if (reaction == reactionType.heal)
        {
            heal();
        }
        else if (reaction == reactionType.terrain)
        {
            removeGate();
        }
    }

    
    public void damageEnemies()
    {
        List<CharacterStats> characters = new List<CharacterStats>();
        foreach(OverlayTile tile in inRangeTiles)
        {
            //tile animation
            if (tile.currentChar != null && tile.currentChar.tag == "Enemy Team")
            {
                battleManager.attack(cursor.character, tile.currentChar);
                battleManager.showDamageNoInvoke();
                battleManager.doDamage(cursor.character, damageOrHealInt);
            }
        }
    }


    public void heal()
    {
        foreach (CharacterStats character in playerTeam.GetComponentsInChildren<CharacterStats>())
        {
            if ((character.health + damageOrHealInt) <= character.maxHealth)
            {
                //Add green + image
                character.health += damageOrHealInt;
            }
            else
            {
                //Add green + image
                int healthAdd = character.maxHealth - character.health;
                character.health += healthAdd;
            }
        }
    }

    public void removeGate()
    {
        gate = false;
    }
}
