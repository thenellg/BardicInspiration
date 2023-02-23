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
    public GameObject actionButtonHolder;
    public GameObject visibleMagicMenu;
    public GameObject spellButton;

    public Transform menuLocation;
    public Camera cam;

    public GameObject TurnOrderParent;
    public List<ShowTurnInfo> visibleTurns = new List<ShowTurnInfo>();
    public SetUpTurnInfo turnInfo;
    public RectTransform background;
    public List<Interactive> Interactables = new List<Interactive>();

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
        float backgroundLength = 50.57f;

        for (int i = 0; i < cursor.character.spells.Length; i++)
        {
            GameObject newButton = Instantiate(spellButton);
            newButton.transform.parent = visibleMagicMenu.transform;
            newButton.transform.localPosition = new Vector3(95.2f, y, 0f);
            newButton.transform.localScale = Vector3.one;

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = cursor.character.spells[i].spellName;
            //set action to spell
            y -= 37.2f;

            if (i != 0)
                backgroundLength += 35f;
        }

        GameObject backButton = Instantiate(spellButton);
        backButton.transform.parent = visibleMagicMenu.transform;
        backButton.transform.localPosition = new Vector3(95.2f, y, 0f);
        backButton.transform.localScale = Vector3.one;

        backButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
        backButton.GetComponent<Button>().onClick.AddListener(delegate { visibleMagicMenu.SetActive(false); });
        backButton.GetComponent<Button>().onClick.AddListener(destroyMagicMenu);
        backButton.GetComponent<Button>().onClick.AddListener(setActionMenu);
        backButton.GetComponent<Button>().onClick.AddListener(delegate { actionButtonHolder.SetActive(true); });

        backgroundLength += 35f;
        background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundLength);

    }

    public void destroyMagicMenu()
    {
        foreach (Transform child in visibleMagicMenu.GetComponentsInChildren<Transform>())
        {
            //Need to account for ignoring the back button
            if (child.gameObject.name == "Spell Button(Clone)")
                Destroy(child.gameObject);
        }
    }

    public void Interact(OverlayTile location)
    {
        foreach(Interactive ruin in Interactables)
        {
            if(ruin == location)
            {
                ruin.GetComponent<Interactive>().Interaction();
                break;
            }
        }
    }

    public void setActionMenu()
    {
        float backgroundLength = 50.57f;
        float y = -23.5f;
        if (cursor.character.activeTile.puzzleSpace)
        {
            GameObject newButton = Instantiate(spellButton);
            newButton.transform.parent = actionButtonHolder.transform;
            newButton.transform.localPosition = new Vector3(95.2f, y, 0f);
            newButton.transform.localScale = Vector3.one;

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ruin";
            newButton.name = "Ruin Button";
            //set action
            newButton.GetComponent<Button>().onClick.AddListener(delegate { Interact(cursor.character.activeTile); });

            y -= 37.2f;
            backgroundLength += 35f;
        }


        for (int i = 0; i < 3; i++)
        {
            GameObject newButton = Instantiate(spellButton);
            newButton.transform.parent = actionButtonHolder.transform;
            newButton.transform.localPosition = new Vector3(95.2f, y, 0f);
            newButton.transform.localScale = Vector3.one;
            
            if(i == 0)
            {
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
                newButton.name = "Attack Button";

                newButton.GetComponent<Button>().onClick.AddListener(battleManager.findAttackTargets);
                newButton.GetComponent<Button>().onClick.AddListener(delegate { visibleActionMenu.SetActive(false); });
            }
            else if(i == 1)
            {
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Magic";
                newButton.name = "Magic Button";

                newButton.GetComponent<Button>().onClick.AddListener(delegate { visibleMagicMenu.SetActive(true); });
                newButton.GetComponent<Button>().onClick.AddListener(setMagicMenu);
                newButton.GetComponent<Button>().onClick.AddListener(destroyActionMenu);
                newButton.GetComponent<Button>().onClick.AddListener(delegate { actionButtonHolder.SetActive(false); });
            }
            else if(i == 2)
            {
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Turn";
                newButton.name = "End Turn Button";

                newButton.GetComponent<Button>().onClick.AddListener(destroyActionMenu);
                newButton.GetComponent<Button>().onClick.AddListener(battleManager.endTurn);
            }

            y -= 37.2f;
            if(i != 0)
                backgroundLength += 35f;
        }
        background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundLength);
    }

    public void destroyActionMenu()
    {
        foreach (Transform child in actionButtonHolder.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name != "Action Menu")
                Destroy(child.gameObject);
        }
    }
}
