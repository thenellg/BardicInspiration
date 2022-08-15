using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public bool customized = false;
    public int characterID;

    [Header("Vitals")]
    public string characterName;
    public OverlayTile activeTile;
    public int health;
    public int maxHealth;

    public int attackRangeMin;
    public int attackRangeMax;
    public Transform menuLocation;
    public Sprite characterPicture;

    [Header("Stats")]
    public int speed;
    public int attack;
    public int defense;
    public int special;

    private void Start() 
    { 
        maxHealth = health;
    }

    public Transform returnMenuLocation()
    {
        return menuLocation;
    }

}
