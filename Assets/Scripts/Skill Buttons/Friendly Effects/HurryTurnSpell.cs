using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurryTurnSpell : SkillButton
{
    private Pawn targetPawn;

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        HurryTurn_Effect hurryTurn_Effect = new HurryTurn_Effect(targetPawn.TurnInfo);
        

        base.OnButtonClick();
    }
}
