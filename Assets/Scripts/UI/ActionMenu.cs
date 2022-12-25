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
    public GameObject visibleMagicMenu;
    public GameObject spellButton;

    public Transform menuLocation;
    public Camera cam;

    public GameObject TurnOrderParent;
    public List<ShowTurnInfo> visibleTurns = new List<ShowTurnInfo>();
    public SetUpTurnInfo turnInfo;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        visibleActionMenu.SetActive(false);
        visibleMagicMenu.SetActive(false);
        battleManager = FindObjectOfType<BattleManager>();
        turnInfo = GetComponentInChildren<SetUpTurnInfo>();

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

    public void setUpInfo()
    {
        for (int i = 0; i < visibleTurns.Count; i++)
        {
            CharacterStats temp = battleManager.turnOrder[i].GetComponent<CharacterStats>();

            visibleTurns[i].setImage(temp.characterPicture);
            visibleTurns[i].setHealth(temp.health, temp.maxHealth);

            if (temp.tag == "Player Team")
                visibleTurns[i].setName(temp.characterName, battleManager.settings.TeammateHighlight);
            else if (temp.tag == "Enemy Team")
                visibleTurns[i].setName(temp.characterName, battleManager.settings.targetHighlight);
            else
                visibleTurns[i].setName(temp.characterName, Color.yellow);
        }
    }

    public void updateTurnsDeath(float height)
    {
        int characterIndex = battleManager.turnNumber;
        if (characterIndex < 0)
            characterIndex = 0;

        Debug.Log("Character Index: " + characterIndex);

        ShowTurnInfo playerInfo;

        for (int i = 0; i < visibleTurns.Count; i++)
        {

            playerInfo = visibleTurns[i];

            if(playerInfo.newY < height)
            {
                playerInfo.newY = playerInfo.transform.localPosition.y + turnInfo.generalMovement;
                playerInfo.moving = true;
            }
        }
    }

    public void updateTurnInfo()
    {
        //foreach (ShowTurnInfo playerInfo in visibleTurns

        int characterIndex = battleManager.turnNumber;
        if (characterIndex < 0)
            characterIndex = 0;

        Debug.Log("Character Index: " + characterIndex);

        ShowTurnInfo playerInfo;

        for (int i = 0; i < visibleTurns.Count; i++)
        {
            if (characterIndex >= battleManager.turnOrder.Count)
                characterIndex = 0;

            playerInfo = visibleTurns[characterIndex];

            if (characterIndex == battleManager.turnNumber - 1 || (battleManager.turnNumber == 0 && characterIndex == battleManager.turnOrder.Count - 1))
            {
                playerInfo.newY = playerInfo.transform.localPosition.y - ((turnInfo.generalMovement * (visibleTurns.Count - 2)) + turnInfo.largerMovement);
                playerInfo.newSize = turnInfo.generalScale;
                playerInfo.sizeChanging = true;
            }
            else if (characterIndex == battleManager.turnNumber || (characterIndex == 0 && battleManager.turnNumber == battleManager.turnOrder.Count) )
            {
                playerInfo.newY = playerInfo.transform.localPosition.y + turnInfo.largerMovement;
                playerInfo.newSize = turnInfo.largerScale;
                playerInfo.sizeChanging = true;
            }
            else
            {
                playerInfo.newY = playerInfo.transform.localPosition.y + turnInfo.generalMovement;
                playerInfo.sizeChanging = false;
            }

            playerInfo.moving = true;

            characterIndex++;
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

    public void setMagicMenu()
    {
        float y = -23.5f;
        for (int i = 0; i < mouseController.character.spells.count; i++)
        {
            GameObject newButton = Instantiate(spellButton);
            newButton.transform.parent = visibleMagicMenu.transform;
            newButton.transform.localPosition = new Vector3(95.2f, y, 0f);
            y -= 37.2f;
        }
    }

    public void destroyMagicMenu()
    {
        foreach (transform child in visibleMagicMenu.GetComponentsinChildren(transform))
        {
            child.gameObject.Destroy();
        }
    }
}
