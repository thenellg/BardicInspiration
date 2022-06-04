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

    public Transform menuLocation;
    public Camera cam;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        visibleActionMenu.SetActive(false);
    }

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

        visibleActionMenu.SetActive(false);
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
