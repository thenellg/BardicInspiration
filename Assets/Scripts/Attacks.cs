using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : ScriptableObject
{
	public CharacterStats characterStats;
	
    public int range; //1 is next to player, etc.
    public bool powerAttack = false;
    public bool magicAttack = false;

    public bool playerAttack = true;
	public bool healingAbility = false;
    //Will need to determine if an enemy is range
	
	void Start()
	{
		attacker = GetComponent<characterStats>();
	}
	
	void attackCheck()
	{
		List<GameObject> charactersInRange = new List<GameObject>();
		
		//check and show in range tiles
		//if blocked, change the color of the square to account for team
		
		
		foreach(var character in charactersInRange){
			if(playerAttack && character.tag == "Player Team")
			{
					charactersInRange.remove(character);
			}
			else if(!playerAttack && character.tag == "Enemy Team")
			{
					charactersInRange.remove(character);
			} 
		}
	}
	
	public List<OverlayTile> GetTilesinRange(OverlayTile startTile, int range)
    {
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        inRangeTiles.Add(startTile);

        int stepCount = 0;

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startTile);

        while(stepCount < range)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach(OverlayTile item in tileForPreviousStep)
            {
                surroundingTiles.AddRange(MapManager.Instance.GetNeighborTiles(item, new List<OverlayTile>()));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
    }
}
