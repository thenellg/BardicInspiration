using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public MouseController cursor;
    private BattleManager battleManager;

    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<MouseController>();
        battleManager = cursor.battleManager;
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
                checkA = testA;

                //if both are in range, check which is the weaker enemy or if the enemy has a type advantage

                nearestCharacter = newTile;
            }
        }

        //move as close to them as possible
        OverlayTile closestTile = cursor.inRangeTiles[0];
        int checkB = cursor.pathfinder.GetGridDistance(nearestCharacter, cursor.inRangeTiles[0]);

        foreach (OverlayTile closest in cursor.inRangeTiles)
        {
            int testB = cursor.pathfinder.GetGridDistance(nearestCharacter, closest);
            if (testB < checkB && testB != 0)
            {
                Debug.Log(testB + " vs " + checkB);
                checkB = testB;
                closestTile = closest;
            }
        }
        cursor.path = cursor.pathfinder.FindPath(cursor.character.activeTile, closestTile, cursor.inRangeTiles);
        cursor.isMoving = true;

    }

    public void enemyAttackCheck()
    {
        List<OverlayTile> neighborTiles = MapManager.Instance.GetNeighborTiles(cursor.character.activeTile, cursor.inRangeTiles);

    }
}
