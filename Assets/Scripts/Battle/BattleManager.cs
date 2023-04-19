using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BattleManager : MonoBehaviour
{
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

    public List<GameObject> magicReactionMinigames = new List<GameObject>();

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
            attackedOnTurn = true;
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
            attackedOnTurn = true;
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
        else if (enemyTeam.Count == 0)
        {
            resultsText.color = settings.TeammateHighlight;
            resultsText.text = "YOU WIN";
            cursor.gameActive = false;
            gameUI.SetActive(false);
            actionMenu.enabled = false;
        }
        else
        {
            if(!onRuin)
                attacker = null;
            defender = null;
            actionMenu.setUpInfo();
        }

        attacking = false;
        Invoke("setAttacking", 3);
    }

    void setAttacking()
    {
        visAttacking = false;
    }

    public void attack(CharacterStats m_Attacker, CharacterStats m_Defender)
    {
        Debug.Log("attack");
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

    public void attackMultiple(CharacterStats m_Attacker, List<CharacterStats> defenders, int damage)
    {
        damageAmount = damage;
        onRuin = true;

        foreach (CharacterStats enemy in defenders)
        {
            defender = enemy;

            showDamageNoInvoke();
            doDamage();
        }

        onRuin = false;
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
        string info = "-" + damageAmount.ToString();
        damage = Instantiate(damageNumbers).GetComponent<TextMeshProUGUI>();
        //Set info
        damage.enabled = false;
        damage.gameObject.transform.parent = gameUI.transform;
        damage.text = info;
        damage.color = settings.damageColor;
        damage.gameObject.transform.position = actionMenu.cam.WorldToScreenPoint(defender.damageLocation.position);

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
