using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionMenu : MonoBehaviour
{
    public MouseController cursor;
    public BattleManager battleManager;

    public GameObject visibleActionMenu;

    public Transform menuLocation;
    public Camera cam;

    public GameObject TurnOrderParent;
    public List<ShowTurnInfo> visibleTurns = new List<ShowTurnInfo>();
    public SetUpTurnInfo turnMenu;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        visibleActionMenu.SetActive(false);
        battleManager = FindObjectOfType<BattleManager>();
        turnMenu = GetComponentInChildren<SetUpTurnInfo>();

        TurnOrderParent.GetComponent<SetUpTurnInfo>().createUI(battleManager.turnOrder.Count);
        int j = 0;
        foreach (ShowTurnInfo turn in TurnOrderParent.GetComponentsInChildren<ShowTurnInfo>())
        {
            if (j < battleManager.turnOrder.Count)
            {
                visibleTurns.Add(turn);
                j++;
            }
            else
            {
                turn.gameObject.SetActive(false);
            }
        }
    }

    public void updateInfo()
    {
        updateTurnInfo();
        visibleActionMenu.SetActive(false);
    }

    public void updateTurnInfo()
    {
        int characterIndex = battleManager.turnNumber;

        /*
         * Original way of adjusting stuff
        
        for(int i = 0; i < visibleTurns.Count; i++)
        {
            if (characterIndex >= battleManager.turnOrder.Count)
                characterIndex = 0;

            CharacterStats temp = battleManager.turnOrder[characterIndex].GetComponent<CharacterStats>();
            
            visibleTurns[i].setImage(temp.characterPicture);
            visibleTurns[i].setHealth(temp.health, temp.maxHealth);

            if (temp.tag == "Player Team")
                visibleTurns[i].setName(temp.characterName, battleManager.settings.TeammateHighlight);
            else if (temp.tag == "Enemy Team")
                visibleTurns[i].setName(temp.characterName, battleManager.settings.targetHighlight);
            else
                visibleTurns[i].setName(temp.characterName, Color.yellow);

            if(cursor.gameActive)
                characterIndex++;
        }
        */

        foreach (ShowTurnInfo playerInfo in visibleTurns)
        {
            if(playerInfo.transform.GetSiblingIndex() == 0) 
            {
                playerInfo.newY = playerInfo.transform.position.y - ((turnMenu.generalMove * (visibleTurns.Count - 2)) + turnMenu.largerMove);
                playerInfo.newSize = turnMenu.generalScale;
            }
            else if(playerInfo.transform.GetSiblingIndex() == 1)
            {
                playerInfo.newY = playerInfo.transform.position.y + turnMenu.largerMovement;
                playerInfo.newSize = turnMenu.largerScale;
            }
            else 
            {
                playerInfo.newY = playerInfo.transform.position.y + turnMenu.generalMove;
            }
            playerInfo.moving = true;
        } 

    }

    public void setActionMenuLocation(CharacterStats character)
    {
        if (cam.WorldToScreenPoint(character.transform.position).x > Screen.width / 2)
        {
            character.menuLocation.localPosition = new Vector3(Mathf.Abs(character.menuLocation.localPosition.x), character.menuLocation.localPosition.y, character.menuLocation.localPosition.z);
        }
        else
        {
            character.menuLocation.localPosition = new Vector3(-1 * Mathf.Abs(character.menuLocation.localPosition.x), character.menuLocation.localPosition.y, character.menuLocation.localPosition.z);
        }

        visibleActionMenu.transform.position = cam.WorldToScreenPoint(character.returnMenuLocation().position);
    }

}
