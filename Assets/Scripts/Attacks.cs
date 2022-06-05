using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
	public CharacterStats characterStats;
	
    public bool powerAttack = false;
    public bool magicAttack = false;

    public bool playerAttack = true;
	public bool healingAbility = false;
	//Will need to determine if an enemy is range
	private RangeFinder rangeFinder;
	private MouseController cursor;

	void Start()
	{
		characterStats = GetComponent<CharacterStats>();
		cursor = FindObjectOfType<MouseController>();
		rangeFinder = new RangeFinder();
	}

	public void attackCheck()
	{
		List<CharacterStats> charactersInRange = GetTilesinRange(characterStats.activeTile, characterStats.attackRangeMin, characterStats.attackRangeMax);
		
		if (charactersInRange.Count == 0)
        {
			Debug.Log("No enemies within range");
        }
        else
        {
			List<OverlayTile> tiles = new List<OverlayTile>();
			foreach (CharacterStats character in charactersInRange)
			{
				Debug.Log(character);
				tiles.Add(character.activeTile);
			}
			cursor.inRangeTiles = tiles;

			if (tiles != null)
				cursor.inRangeTiles = tiles;
		}
	}
	
	
	public List<CharacterStats> GetTilesinRange(OverlayTile startTile, int minRange, int maxRange)
	{
		List<CharacterStats> enemies = new List<CharacterStats>();
		foreach(OverlayTile tile in rangeFinder.GetEnemiesinRange(startTile, minRange, maxRange))
        {
			tile.ShowTile();
			if(tile.isBlocked && tile.currentChar.tag == "Enemy Team" && playerAttack)
            {
				enemies.Add(tile.currentChar);
            }
			else if (tile.isBlocked && tile.currentChar.tag == "Player Team" && !playerAttack)
			{
				enemies.Add(tile.currentChar);
			}
        }
		return enemies;
    }
	
}
