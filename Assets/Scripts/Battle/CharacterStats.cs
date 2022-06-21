using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
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

        GameSettings settings = FindObjectOfType<GameSettings>();
        setStats(settings);
    }

    void setStats(GameSettings settings)
    {
        /*
        public string characterName;
    public int health;
    public int attackRangeMin;
    public int attackRangeMax;
    public Sprite characterPicture;
    public int speed;
    public int attack;
    public int defense;
    public int special;
    */
}

    public Transform returnMenuLocation()
    {
        return menuLocation;
    }

}
