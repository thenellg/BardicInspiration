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

    public float newY = 0;
    public bool moving = false;
    public bool sizeChanging = false;
    public float newSize;
    public float currentSize;
    private RectTransform turnInfoPosition;

    void Start()
    {
        currentSize = this.gameObject.transform.localScale.x;
        newSize = currentSize;
    }

    void Update()
    {
        if(moving = true)
        {
            if (turnInfoPosition.position.y != newY)
            {
                turnInfoPosition.position = Vector3.MoveTowards(turnInfoPosition.position, new Vector3(turnInfoPosition.positon.x, newY, turnInfoPosition.position.z), 1);
            }
            else
            {
                moving = false;
            }
        }

        //if(currentSize != newSize)
        //{

        //}
    }

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
