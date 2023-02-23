using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Header("Colors")]
    public Color CanMoveHighlight = new Color(255, 255, 255);
    public Color directionArrow = new Color(255, 246, 115);
    public Color targetHighlight = new Color(253, 45, 55);
    public Color TeammateHighlight = new Color(14, 119, 191);
    public Color cursor = new Color(255, 122, 0);
    public Color damageColor = new Color(253, 45, 55);
    public Color healingColor = new Color(114, 185, 46);

    [Header("Player Stats")]
    public int customCharacterID;
    public CharStat character;

    void Awake()
    {
        GameSettings[] objs = FindObjectsOfType<GameSettings>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void setUp(CharacterStats hero)
    {
        hero.characterName = character.characterName;
        hero.health = character.health;
        hero.attackRangeMin = character.attackRangeMin;
        hero.attackRangeMax = character.attackRangeMax;
        hero.speed = character.speed;
        hero.attack = character.attack;
        hero.defense = character.defense;
        hero.special = character.special;
        hero.characterPicture = character.characterPicture;
    }
}
