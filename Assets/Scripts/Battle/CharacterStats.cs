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
        GameSettings settings = FindObjectOfType<GameSettings>();
        if(customized)
            setStats(settings);

        maxHealth = health;
    }

    void setStats(GameSettings settings)
    {
        characterName = settings.character.characterName;
        health = settings.character.health;
        attackRangeMin = settings.character.attackRangeMin;
        attackRangeMax = settings.character.attackRangeMax;
        speed = settings.character.speed;
        attack = settings.character.attack;
        defense = settings.character.defense;
        special = settings.character.special;
        characterPicture = settings.character.characterPicture;

    }

    public Transform returnMenuLocation()
    {
        return menuLocation;
    }

}
