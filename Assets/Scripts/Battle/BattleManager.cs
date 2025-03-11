using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BattleManager : MonoBehaviour
{
    public enum winConditions { defeatEnemies, defeatSingleEnemy, position }


    public List<GameObject> turnOrder;
    public List<GameObject> playerTeam;
    public List<GameObject> enemyTeam;
    public List<GameObject> interactives;
    public int turnNumber = 0;
    public ActionMenu actionMenu;
    public MouseController cursor;
    public TextMeshProUGUI resultsText;
    public int damageAmount = 0;
    public bool onRuin = false;
    public GameObject tileContainer;
    public winConditions m_winConditions;

    public bool attacking = false;
    public bool magicAttacking = false;
    public bool visAttacking = false;
    private bool attackedOnTurn = false;

    public List<OverlayTile> playerLocations;
    public GameSettings settings;
    public GameObject gameUI;
    public GameObject damageNumbers;
    
    private TextMeshProUGUI damage;
    public CharacterStats attacker;
    public CharacterStats defender;
    public Spell currentSpell;

    public bool magicMiniGameSuccess = false;

    public CharacterStats winEnemy;
    public List<Vector2> winTiles;
    public bool won = false;

    private void Start()
    {
        MouseController temp = FindObjectOfType<MouseController>();
        settings = FindObjectOfType<GameSettings>();

        //play battle start animation
        //Invoke startSequence at end of animation
        startSequence();
        //Debug.Log(turnOrder.Count);
        resultsText.text = " ";
    }

    void startSequence()
    {
        actionMenu.setUpInfo();
        onTurnSwap();

        if (turnOrder[0].GetComponent<CharacterStats>().tag == "Player Team")
        {
            cursor.activeMovement = true;
            cursor.GetInRangeTiles();
        }
        else if (turnOrder[0].GetComponent<CharacterStats>().tag == "Enemy Team")
        {
            cursor.character.GetComponent<BasicEnemy>().enemyMove();

            Invoke("enemyMove", 2f);
        }
    }

    public void updatePlayerLocations()
    {
        playerLocations.Clear();
        foreach(var character in playerTeam)
        {
            if (character.activeSelf)
                playerLocations.Add(character.GetComponent<CharacterStats>().activeTile);
        }
    }

    public bool isRoundOver()
    {
        if (enemyTeam.Count == 0)
        {
            //Game Over Win
            return true;
        }
        else if (playerTeam.Count == 0)
        {
            //Game Over Lose
            return true;
        }

        return false;
    }

    public void findAttackTargets()
    {
        if (!attackedOnTurn)
        {
            cursor.cursorActive = true;
            attacking = cursor.character.GetComponent<Attacks>().attackCheck();
        }
        else
        {
            endTurn();
        }
    }

    public void findMagicTargets()
    {
        if (!attackedOnTurn)
        {
            //Will have to adjust this for magic checks
            magicAttacking = cursor.character.GetComponent<Attacks>().attackCheck();
            //attackedOnTurn = true;
        }
        else
        {
            endTurn();
        }
    }

    public void deathCheck()
    {
        if (defender.health <= 0)
        {
            defender.activeTile.isBlocked = false;
            defender.activeTile.currentChar = null;

            turnOrder.Remove(defender.gameObject);

            if (playerTeam.Contains(defender.gameObject))
                playerTeam.Remove(defender.gameObject);
            else if (enemyTeam.Contains(defender.gameObject))
                enemyTeam.Remove(defender.gameObject);

            turnNumber = turnOrder.IndexOf(attacker.gameObject);


            defender.gameObject.SetActive(false);
            if (defender.tag == "Player Team")
                updatePlayerLocations();

            int index = 0;
            float height = 0;
            for (int i = 0; i < actionMenu.visibleTurns.Count; i++)
            {
                if (actionMenu.visibleTurns[i].characterName.text == defender.characterName)
                    index = i;
            }
            height = actionMenu.visibleTurns[index].newY;

            Debug.Log("Delete index = " + index);

            GameObject temp = actionMenu.visibleTurns[index].gameObject;
            actionMenu.visibleTurns.RemoveAt(index);
            Destroy(temp);

            actionMenu.updateTurnsDeath(height);

            if(m_winConditions == winConditions.defeatSingleEnemy && defender == winEnemy)
            {
                won = true;
            }

            //Get Player Turn UI element
            //Delete it
            //Move all showing below it

            //temp.SetActive(false);
        }
    }

    public void doDamage()
    {
        if (!onRuin)
            damageAmount = attacker.attack;

        if (defender.health - damageAmount > defender.maxHealth)
            defender.health = defender.maxHealth;
        else
            defender.health -= damageAmount;

        deathCheck();

        if (playerTeam.Count == 0)
        {
            resultsText.color = settings.targetHighlight;
            resultsText.text = "YOU LOSE";
            cursor.gameActive = false;
            gameUI.SetActive(false);
            actionMenu.enabled = false;
        }
        else if (enemyTeam.Count == 0 && m_winConditions == winConditions.defeatEnemies || won == true)
        {
            endGame();
        }
        else
        {
            if(!onRuin)
                attacker = null;
            defender = null;
            actionMenu.setUpInfo();

            attacking = false;
            Invoke("setAttacking", 3);
        }
    }

    public void endGame()
    {
        resultsText.color = settings.TeammateHighlight;
        resultsText.text = "YOU WIN";
        cursor.gameActive = false;
        gameUI.SetActive(false);
        actionMenu.enabled = false;
    }

    void setAttacking()
    {
        visAttacking = false;
    }

    public void attack(CharacterStats m_Attacker, CharacterStats m_Defender)
    {
        Debug.Log("attack");
        attackedOnTurn = true;
        
        attacker = m_Attacker; defender = m_Defender;
        attacking = true;
        visAttacking = true;

        //Add stuff here to actually make the attack cool;
        defender.activeTile.HideTile();

        //Get vector between attacker and defender
        //Turn defender red and move them
        m_Defender.characterSprite.color = settings.damageColor;
        
        Vector3 direction =  m_Defender.transform.position - m_Attacker.transform.position;
        direction.Normalize();
        direction = new Vector3(direction.x * 0.0005f, direction.y * 0.0005f, direction.z * 0.0005f);

        m_Defender.GetComponent<CharacterAnimationHandler>().setDamageMove(direction.x, direction.y);
        attacking = false;

        if (onRuin)
            resetDefender();
        else
            Invoke("resetDefender", 0.2f);

    }

    public void magicAttack(List<OverlayTile> range)
    {
        attacker = cursor.character;
        onRuin = true;

        damageAmount = currentSpell.attackDamage;

        if (magicMiniGameSuccess == true) {
            int rand = UnityEngine.Random.Range(2, 4);
            damageAmount += rand;
            magicMiniGameSuccess = false;
        }

        foreach (OverlayTile tile in range)
        {
            if (playerTeam.Contains(attacker.gameObject))
            {
                if (tile.currentChar != null && tile.currentChar.tag == "Enemy Team")
                {
                    defender = tile.currentChar;

                    showDamageNoInvoke();
                    doDamage();
                }
                else if (tile.currentChar != null && tile.currentChar.tag == "Player Team" && currentSpell.spellType == Spell.spellTypes.Buff)
                {
                    defender = tile.currentChar;
                    damageAmount *= -1;

                    showDamageNoInvoke();
                    doDamage();
                }
            }
            else if (enemyTeam.Contains(attacker.gameObject))
            {
                if (tile.currentChar != null && tile.currentChar.tag == "Player Team")
                {
                    defender = tile.currentChar;

                    showDamageNoInvoke();
                    doDamage();
                }
                else if (tile.currentChar != null && tile.currentChar.tag == "Enemy Team" && currentSpell.spellType == Spell.spellTypes.Buff)
                {
                    defender = tile.currentChar;
                    damageAmount *= -1;

                    showDamageNoInvoke();
                    doDamage();
                }
            }
        }

        attackedOnTurn = true;

        foreach (OverlayTile tile in cursor.inRangeTiles)
            tile.HideTile();
        foreach (OverlayTile tile in cursor.magicRangeTiles)
            tile.HideTile();


        actionMenu.visibleTurns[turnNumber].updateSpellSlots(cursor.character.spellSlots);

        actionMenu.actionButtonHolder.SetActive(true);
        actionMenu.setActionMenu();

        cursor.postMagicReset();
        Invoke("endTurn", 1f);
    }

    public void attackMultiple(CharacterStats m_Attacker, List<CharacterStats> defenders, int damage)
    {
        damageAmount = damage;
        onRuin = true;

        foreach (CharacterStats enemy in defenders)
        {
            defender = enemy;

            showDamage();
        }

        onRuin = false;
    }

    public void beginSpellAttack()
    {
        if (attackedOnTurn == false && cursor.character.spellSlots > 0)
        {
            actionMenu.destroyMagicMenu();
            actionMenu.visibleMagicMenu.SetActive(false);
            actionMenu.visibleActionMenu.SetActive(false);
            
            cursor.character.spellSlots -= currentSpell.cost;


            List<OverlayTile> range = new List<OverlayTile>();

            if (currentSpell.spellType == Spell.spellTypes.AreaOfEffect)
            {
                range = cursor.rangeFinder.GetTilesinRange(cursor.character.activeTile, currentSpell.maxSpellRange);
                foreach (OverlayTile tile in range)
                    tile.ShowTile(true);
                cursor.inRangeTiles = range;
            }
            else if (currentSpell.spellType == Spell.spellTypes.Line)
            {
                Vector2 baseTile = cursor.character.activeTile.gridLocation2D;
                foreach(OverlayTile tile in tileContainer.GetComponentsInChildren<OverlayTile>())
                {
                    if ( (tile.gridLocation2D.x <= (baseTile.x + currentSpell.minSpellRange/2)) && (tile.gridLocation2D.x >= (baseTile.x - currentSpell.minSpellRange/2)) ) 
                    {
                        range.Add(tile);
                        tile.ShowTile(true);
                    }
                    else if ((tile.gridLocation2D.y <= (baseTile.y + currentSpell.minSpellRange / 2)) && (tile.gridLocation2D.y >= (baseTile.y - currentSpell.minSpellRange / 2)))
                    {
                        range.Add(tile);
                        tile.ShowTile(true);
                    }
                }



                cursor.inRangeTiles = range;
            }
            else if (currentSpell.spellType == Spell.spellTypes.Single)
            {
                foreach(GameObject character in enemyTeam)
                {
                    if(character.GetComponent<CharacterStats>().health > 0)
                    {
                        range.Add(character.GetComponent<CharacterStats>().activeTile);
                    }
                }
                cursor.inRangeTiles = range;
                
                int i = 0;
                foreach (OverlayTile tile in cursor.inRangeTiles)
                {
                    tile.SetColor(settings.targetHighlight);
                    tile.ShowTile(true);
                    i++;
                }
                Debug.Log(i + " tiles should be showing");
            }
            else if (currentSpell.spellType == Spell.spellTypes.Buff)
            {
                cursor.inRangeTiles = playerLocations;
                foreach (OverlayTile tile in playerLocations)
                    tile.ShowTile(true);

            }

            magicAttacking = true;
            cursor.activeMovement = true;
            cursor.cursorActive = true;
        }
        else
        {
            endTurn();
        }
    }

    void resetDefender()
    {
        if (defender.health - damageAmount > 0)
        {
            defender.characterSprite.color = Color.white;
            showDamage();

            defender.GetComponent<CharacterAnimationHandler>().setDamageMoveBack(defender.activeTile.characterPos.position);
        }
        else
        {
            showDamage();
            defender.GetComponent<CharacterAnimationHandler>().stopDamageMove();
        }
    }

    public void showDamage()
    {
        showDamageNoInvoke();
        Invoke("doDamage", 1f);
    }

    public void showDamageNoInvoke()
    {
        if (damageAmount > 0)
        {
            string info = "-" + damageAmount.ToString();
            damage = Instantiate(damageNumbers).GetComponent<TextMeshProUGUI>();
            //Set info
            damage.enabled = false;
            damage.gameObject.transform.parent = gameUI.transform;
            damage.text = info;
            damage.color = settings.damageColor;
            damage.gameObject.transform.position = actionMenu.cam.WorldToScreenPoint(defender.damageLocation.position);
        }
        else
        {
            string info = "-" + (damageAmount * -1).ToString();
            damage = Instantiate(damageNumbers).GetComponent<TextMeshProUGUI>();
            //Set info
            damage.enabled = false;
            damage.gameObject.transform.parent = gameUI.transform;
            damage.text = info;
            damage.color = settings.healingColor;
            damage.gameObject.transform.position = actionMenu.cam.WorldToScreenPoint(defender.damageLocation.position);

        }
        //Show and animate
        damage.enabled = true;
        damage.GetComponent<damageNumbers>().move = true;
    }

    public void endTurn()
    {
        //Check isRoundOver(), if not

        //Moving to next turn
        turnNumber++;
        if (turnNumber == turnOrder.Count)
            turnNumber = 0;

        cursor.character = turnOrder[turnNumber].GetComponent<CharacterStats>();
        attackedOnTurn = false;

        Debug.Log(cursor.character);
        onTurnSwap();
        cursor.cursorActive = true;

        Invoke("finishEndTurn", 0.5f);

    }

    void finishEndTurn()
    {
        if (cursor.character.tag == "Player Team")
        {
            cursor.activeMovement = true;
        }
        else if (cursor.character.tag == "Enemy Team")
        {
            //Eventually set this up for account for different types of Enemy AIs
            cursor.character.GetComponent<BasicEnemy>().enemyMove();

            Invoke("enemyMove", 2f);
        }
    }

    void enemyMove()
    {
        endTurn();
    }

    public void onTurnSwap()
    {
        actionMenu.updateInfo();
        cursor.GetInRangeTiles();
    }

}
