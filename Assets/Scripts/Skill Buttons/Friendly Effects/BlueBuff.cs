using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBuff : SkillButton
{
    private Pawn targetPawn;

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;
        if(targetPawn.DoDoubleTurn)
        {
            return;
        }
        targetPawn.DoDoubleTurn = true;
        // targetPawn.ApplySpecialEffect(effectIcon, "Blue");
        targetPawn.AddEffectIcon(effectIcon, "blueBuff");

        BattleLogVerticalGroup.Instance.AddPsionEntry(targetPawn.Name, PsionActionSymbol.Blue, Color.blue);

        base.OnButtonClick();
    }
}
