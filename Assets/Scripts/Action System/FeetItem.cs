using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetItem : ActionItem
{
    TileWalker targetWalker;

    public int currentRangeInTiles = 1;

    LookAtter lookAtter;

    public override void Awake()
    {
        lookAtter = GetComponentInChildren<LookAtter>();
        base.Awake();
    }

    public override void Action(GameObject tgt) //might be depricated
    {
        targetWalker = tgt.GetComponent<TileWalker>(); //Something here is redundant
        GridPoser gridPoser = tgt.GetComponent<GridPoser>(); //Something here is redundant

        lookAtter.tgt = tgt.transform;

        if (gridPoser != null)
        {
            pawn.tileWalker.StartNewPathWithRange(gridPoser.GetGridPos());
        }
        else
        {
            pawn.tileWalker.StartNewPathWithRange(targetWalker, currentRangeInTiles);
        }
    }

    public void Action(GameObject tgt, ActionVariation actionOnDestination)
    {
        targetWalker = tgt.GetComponent<TileWalker>(); //Something here is redundant
        GridPoser gridPoser = tgt.GetComponent<GridPoser>(); //Something here is redundant

        lookAtter.tgt = tgt.transform;


        if (gridPoser != null)
        {
            pawn.tileWalker.StartNewPathWithRange(gridPoser.GetGridPos());
        }
        else
        {
            pawn.tileWalker.StartNewPathWithRange(targetWalker, currentRangeInTiles);
        }


    }

    

    
}
