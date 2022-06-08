using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTurnInfo : MonoBehaviour
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHealth;

    public void setImage(Sprite newImage)
    {
        characterImage.sprite = newImage;
    }

    public void setName(string newName, Color stringColor)
    {
        characterName.text = newName;
        characterName.color = stringColor;
    }

    public void setHealth(int health, int maxHealth)
    {
        string temp = health + "/" + maxHealth;
        characterHealth.text = temp;
    }
}
