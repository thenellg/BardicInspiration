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
    public GameObject spellSlots;
    public GameObject spellSlotsPrefab;
    public List<SpellSlot> slots = new List<SpellSlot>();

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

    public void setSpellSlots(bool hero, int maxSlots)
    {
        if (slots.Count < maxSlots)
        {
            float location = 0;
            if (hero)
            {
                for (int i = 0; i < maxSlots; i++)
                {
                    GameObject slot = Instantiate(spellSlotsPrefab);
                    slot.transform.parent = spellSlots.transform;
                    slot.GetComponent<RectTransform>().localPosition = new Vector3(location, -5, 0);
                    location += 15;
                    slots.Add(slot.GetComponent<SpellSlot>());
                }
            }
            else
            {
                Destroy(spellSlots.gameObject);
            }
        }
    }

    public void updateSpellSlots(int numSlotsLeft)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= numSlotsLeft)
                slots[i].GetComponent<SpellSlot>().setUsed();
        }
    }
}
