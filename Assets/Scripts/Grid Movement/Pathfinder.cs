using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            OverlayTile currentTile = openList.OrderBy(x => x.f).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if(currentTile == end)
            {
                //end the path
                return GetFinishedList(start, end);
            }

            List<OverlayTile> neighborTiles = MapManager.Instance.GetNeighborTiles(currentTile, searchableTiles);

            foreach(OverlayTile neighbor in neighborTiles)
            {
                //1 = jump height
                if(neighbor.isBlocked || closedList.Contains(neighbor))
                {
                    continue;
                }

                neighbor.g = GetGridDistance(start, neighbor);
                neighbor.h = GetGridDistance(start, end);

                neighbor.previous = currentTile;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }
        finishedList.Reverse();
        
        return finishedList;
    }

    public int GetGridDistance(OverlayTile start, OverlayTile neighbor)
    {
        return Mathf.Abs(start.gridLocation.x - neighbor.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbor.gridLocation.y);
    }
}
