using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurnSpell : SkillButton
{
    //private Pawn targetPawn;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        DoubleTurn_Effect doubleTurn_Effect = new DoubleTurn_Effect(pawnTgt.TurnInfo, effectIcon);
      

        base.OnButtonClick();
    }
}
