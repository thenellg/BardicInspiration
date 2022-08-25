using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public enum enemyType { Soldier, Archer, Thief, Assassin };

    public MouseController cursor;
    private BattleManager battleManager;
    public CharacterStats character;
    public enemyType m_enemyType;


    public List<OverlayTile> attackRange = new List<OverlayTile>();

    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<MouseController>();
        battleManager = cursor.battleManager;
    }

    public void enemyMove()
    {
        //I'd like to implement some smarter logic here for different states.
        //Like if the enemy is at half health have a chance of running away
        //Or maybe supporting characters. Gotta look into a state machine for that
        //probably. This works in the mean time.

        if (m_enemyType == enemyType.Soldier || m_enemyType == enemyType.Archer)
        {
            moveTowardsClosestPlayer();
        }
        else if (m_enemyType == enemyType.Assassin || m_enemyType == enemyType.Thief)
        {
            moveTowardsWeakestPlayer();
        }
    }

    public void moveTowards(OverlayTile player)
    {
        //move as close to them as possible
        OverlayTile closestTile = cursor.inRangeTiles[0];
        int checkB = cursor.pathfinder.GetGridDistance(player, cursor.inRangeTiles[0]);

        if (checkB > character.attackRangeMin)
        {
            foreach (OverlayTile closest in cursor.inRangeTiles)
            {
                int testB = cursor.pathfinder.GetGridDistance(player, closest);
                if (testB < checkB && testB != 0 && testB >= character.attackRangeMin)
                {
                    //Debug.Log(testB + " vs " + checkB);
                    checkB = testB;
                    closestTile = closest;
                }
            }
        }
        else
        {
            foreach (OverlayTile closest in cursor.inRangeTiles)
            {
                int testB = cursor.pathfinder.GetGridDistance(player, closest);
                if (testB > checkB && testB != 0 && testB <= character.attackRangeMax)
                {
                    //Debug.Log(testB + " vs " + checkB);
                    checkB = testB;
                    closestTile = closest;
                }
            }
        }
        cursor.path = cursor.pathfinder.FindPath(cursor.character.activeTile, closestTile, cursor.inRangeTiles);
        cursor.isMoving = true;
    }

    public void moveTowardsClosestPlayer()
    {
        //Go through battleManager.playerLocations to find the closest player
        int checkA = cursor.pathfinder.GetGridDistance(cursor.character.activeTile, cursor.battleManager.playerLocations[0]);
        OverlayTile nearestCharacter = battleManager.playerLocations[0];

        foreach (OverlayTile newTile in battleManager.playerLocations)
        {
            int testA = cursor.pathfinder.GetGridDistance(cursor.character.activeTile, newTile);
            if (testA < checkA)
            {
                //if both are in range, check which is the weaker enemy or if the enemy has a type advantage

                checkA = testA;
                nearestCharacter = newTile;
            }
        }

        moveTowards(nearestCharacter);

    }

    public void moveTowardsWeakestPlayer()
    {
        //Go through battleManager.playerLocations to find the closest player
        int checkA = 100;
        OverlayTile nearestCharacter = battleManager.playerLocations[0];

        foreach (OverlayTile newTile in battleManager.playerLocations)
        {
            int testA = newTile.currentChar.health;
            if (testA < checkA)
            {
                checkA = testA;

                //if both are in range, check which is the weaker enemy or if the enemy has a type advantage

                nearestCharacter = newTile;
            }
        }

        moveTowards(nearestCharacter);

    }

    public OverlayTile enemyAttackCheck()
    {
        //Check if enemy is in range
        attackRange = cursor.rangeFinder.GetTilesinRange(character.activeTile,character.attackRangeMax);
        foreach(OverlayTile tile in attackRange)
        {
            int range = cursor.pathfinder.GetGridDistance(character.activeTile, tile);
            if (tile.isBlocked && tile.currentChar.tag == "Player Team" && range >= character.attackRangeMin)
            {
                return tile;
            }
        }

        return null;
    }
}
