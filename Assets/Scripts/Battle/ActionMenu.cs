using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionMenu : MonoBehaviour
{
    public MouseController cursor;

    public GameObject visibleActionMenu;

    public TextMeshProUGUI turnName;
    public TextMeshProUGUI turnNumber;



    public void updateInfo()
    {
        CharacterStats character = cursor.character;
        turnName.text = "Turn " + (cursor.battleManager.turnNumber + 1).ToString(); ;

        turnName.text = character.name;
        if (character.tag == "Player Team")
            turnName.color = Color.blue;
        else if (character.tag == "Enemy Team")
            turnName.color = Color.red;
        else
            turnName.color = Color.yellow;
    }

}
