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
    public RectTransform turnInfoPosition;

    public bool moving = false;
    public bool sizeChanging = false;
    public float newY = 0;
    public float newSize;
    public float currentSize;

    void Start()
    {
        currentSize = this.gameObject.transform.localScale.x;
        newSize = currentSize;
        newY = turnInfoPosition.localPosition.y;
    }

    void Update()
    {
        if(moving == true)
        {
            if (turnInfoPosition.localPosition.y != newY)
            {
                turnInfoPosition.localPosition = Vector3.MoveTowards(turnInfoPosition.localPosition, new Vector3(turnInfoPosition.localPosition.x, newY, turnInfoPosition.localPosition.z), 1);
            }
            else
            {
                turnInfoPosition.localPosition = new Vector3(turnInfoPosition.localPosition.x, newY, turnInfoPosition.localPosition.z);
                moving = false;
            }
        }

        if(sizeChanging)
        {
            turnInfoPosition.localScale = Vector3.MoveTowards(turnInfoPosition.localScale, new Vector3(newSize, newSize, newSize), 0.1f);
        }
        else
        {
            turnInfoPosition.localScale = new Vector3(newSize, newSize, newSize);
            sizeChanging = false;
        }
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
