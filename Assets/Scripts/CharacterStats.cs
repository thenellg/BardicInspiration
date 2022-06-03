using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Vitals")]
    public string characterName;
    public OverlayTile activeTile;
    public int health;
    public int attackRangeMin;
    public int attackRangeMax;
    public Transform menuLocation;

    [Header("Stats")]
    public int speed;
    public int attack;
    public int defense;
    public int special;

    public Transform returnMenuLocation()
    {
        return menuLocation;
    }

}
