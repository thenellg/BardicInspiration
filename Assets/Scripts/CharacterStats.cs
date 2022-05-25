using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Vitals")]
    public string characterName;
    public OverlayTile activeTile;
    public int health;

    [Header("Stats")]
    public int speed;
    public int attack;
    public int defense;
    public int special;

}
