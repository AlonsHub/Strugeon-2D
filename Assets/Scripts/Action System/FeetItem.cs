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

    public override void Action(GameObject tgt)
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

    public void Action(GameObject tgt, ActionItem atDestination)
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


    //IEnumerator CharacterWalk()
    //{
    //    //ownerCharacter.tileWalker.StartNewPath(actionPool[actionIndex].target.GetComponent<TileWalker>());
    //    //ownerCharacter.tileWalker.StartNewPathWithRange(targetWalker, ownerCharacter.wa);
    //    //BattleLog.Instance.AddLine(name + " Moved towards: " + targetWalker.name);
    //    //BattleLogVerticalGroup.Instance.AddEntry(name, ActionSymbol.Walk, targetWalker.name);

    //    pawn.tileWalker.StartNewPathWithRange(pawn.tileWalker, currentRangeInTiles);

    //    yield return new WaitUntil(() => !pawn.tileWalker.hasPath);
    //    pawn.TurnDone = true;
    //}
}
