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
    public Transform gameUI;
    public bool gameFinished = false;
    //[Header("Reaction")]
    public enum reactionType
    {
        damage, terrain, heal
    }
    public reactionType reaction;
    public bool gate = true;
    public int damageOrHealInt;
    public GameObject playerTeam;
    public float intModifier = 0f;

    public BlockGame m_blockGame;
    public List<CharacterStats> targets = new List<CharacterStats>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interaction()
    {
        Debug.Log("got to Interaction()");
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

    private void Update()
    {
        if (m_blockGame && (m_blockGame.succeeded >= 3 || m_blockGame.endGame))
        {
            if(reaction == reactionType.damage)
            {
                if(m_blockGame.succeeded >= 3)
                    damageOrHealInt = (int)((float)damageOrHealInt * intModifier);

                Destroy(m_blockGame.gameObject);
                doDamage();
            }
        }
    }

    public void showTiles()
    {
        foreach(OverlayTile tile in inRangeTiles)
        {
            tile.showRuinTile();
        }
    }

    public void hideTiles()
    {
        foreach(OverlayTile tile in inRangeTiles)
        {
            if (battleManager.cursor.inRangeTiles.Contains(tile) && !tile.isBlocked)
                tile.GetComponent<SpriteRenderer>().color = battleManager.settings.CanMoveHighlight;
            else 
                tile.HideTile();
        }
    }

    IEnumerator damageMinigame()
    {
        m_blockGame = Instantiate(minigame).GetComponent<BlockGame>();
        m_blockGame.transform.parent = gameUI;
        m_blockGame.transform.localScale = Vector3.one;
        m_blockGame.transform.localPosition = Vector3.zero;
        m_blockGame.ruin = this;
        yield return null;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
    }

    void doDamage()
    {
        Debug.Log("got to damageEnemies()");
        foreach (OverlayTile tile in inRangeTiles)
        {
            //tile animation
            if (tile.currentChar != null && tile.currentChar.tag == "Enemy Team")
            {
                targets.Add(tile.currentChar);
            }
        }

        battleManager.attackMultiple(activeTile.currentChar, targets, damageOrHealInt);

        activeTile.puzzleSpace = false;

        Invoke("endSet", 0.8f);
    }

    public void damageEnemies()
    {
        hideTiles();
        battleManager.attacker = activeTile.currentChar;
        battleManager.actionMenu.visibleActionMenu.SetActive(false);
        battleManager.actionMenu.destroyActionMenu();
        StartCoroutine(damageMinigame());
    }


    public void heal()
    {
        battleManager.actionMenu.visibleActionMenu.SetActive(false);
        battleManager.actionMenu.destroyActionMenu();
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
        activeTile.puzzleSpace = false;
        Invoke("endSet", 0.8f);
    }

    void endSet()
    {
        battleManager.actionMenu.setActionMenu();
        battleManager.actionMenu.visibleActionMenu.SetActive(true);
    }

    public void removeGate()
    {
        gate = false;
    }
}
