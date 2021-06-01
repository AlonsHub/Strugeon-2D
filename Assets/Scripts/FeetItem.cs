using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetItem : ActionItem
{
    TileWalker targetWalker;

    public int currentRangeInTiles = 1;

    public override void Action(GameObject tgt)
    {
        targetWalker = tgt.GetComponent<TileWalker>(); //Something here is redundant
        GridPoser gridPoser = tgt.GetComponent<GridPoser>(); //Something here is redundant

        GetComponentInChildren<LookAtter>().tgt = tgt.transform;

        if (gridPoser != null)
        {
            //GridPoser gridPoser = tgt.GetComponent<GridPoser>();
            pawn.tileWalker.StartNewPathWithRange(gridPoser.GetGridPos());
            //BattleLogVerticalGroup.Instance.AddEntry(name, ActionSymbol.Walk, gridPoser.GetName());
        }
        else
        {
            pawn.tileWalker.StartNewPathWithRange(targetWalker, currentRangeInTiles);
            //BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Walk, targetWalker.pawn.name);
            //BattleLogVerticalGroup.Instance.AddEntry(name, ActionSymbol.Walk, targetWalker.name);
        }
        // BatllelogVerticalGroup.Instance.AddEntry(pawn.name, ActionIcon.Walk, tgt.name);

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
