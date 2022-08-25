using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> turnOrder;
    public List<GameObject> playerTeam;
    public List<GameObject> enemyTeam;
    public int turnNumber = 0;
    public ActionMenu actionMenu;
    public MouseController cursor;
    public TextMeshProUGUI resultsText;


    public bool attacking = false;
    private bool attackedOnTurn = false;
    public List<OverlayTile> playerLocations;
    public GameSettings settings;
    public GameObject gameUI;

    private void Start()
    {
        MouseController temp = FindObjectOfType<MouseController>();
        settings = FindObjectOfType<GameSettings>();

        //play battle start animation
        //Invoke startSequence at end of animation
        startSequence();
        Debug.Log(turnOrder.Count);
        resultsText.text = " ";
    }

    void startSequence()
    {
        onTurnSwap();

        if (turnOrder[0].GetComponent<CharacterStats>().tag == "Player Team")
        {
            cursor.activeMovement = true;
            cursor.GetInRangeTiles();
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
            attacking = cursor.character.GetComponent<Attacks>().attackCheck();
            attackedOnTurn = true;
        }
        else
        {
            endTurn();
        }
    }

    public void attack(CharacterStats attacker, CharacterStats defender)
    {
        //make an animation and show damage numbers

        //Add stuff here to actually make the attack cool;
        defender.health -= attacker.attack;
        defender.activeTile.HideTile();

        //if health <= 0, kill character
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

            GameObject temp = actionMenu.visibleTurns[actionMenu.visibleTurns.Count - 1].gameObject;
            actionMenu.visibleTurns.RemoveAt(actionMenu.visibleTurns.Count - 1);
            temp.SetActive(false);
        }

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
            actionMenu.updateTurnInfo();
        }
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
