using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RangeFinder
{
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

    public List<OverlayTile> GetEnemiesinRange(OverlayTile startTile, int minRange, int maxRange)
    {
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        inRangeTiles.Add(startTile);

        int stepCount = minRange;

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startTile);

        while (stepCount < maxRange)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach (OverlayTile item in tileForPreviousStep)
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
