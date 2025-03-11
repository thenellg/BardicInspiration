using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public bool customized = false;
    public int characterID;
    public SpriteRenderer characterSprite;

    [Header("Vitals")]
    public string characterName;
    public OverlayTile activeTile;
    public bool multiTileNeeded;
    public List<OverlayTile> extraTiles = new List<OverlayTile>();
    public int health;
    public int maxHealth;

    public int attackRangeMin;
    public int attackRangeMax;
    public Transform menuLocation;
    public Transform damageLocation;
    public Sprite characterPicture;

    [Header("Stats")]
    public int speed;
    public int attack;
    public int defense;
    public int special;

    [Header("Spells")]
    public int maxSpellSlots;
    public int spellSlots;
    public Spell[] spells;

    private void Start() 
    { 
        maxHealth = health;
        characterSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public Transform returnMenuLocation()
    {
        return menuLocation;
    }

}
