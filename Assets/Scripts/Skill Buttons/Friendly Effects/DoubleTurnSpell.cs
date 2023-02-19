using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurnSpell : SkillButton
{
    private Pawn targetPawn;

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;

        DoubleTurn_Effect doubleTurn_Effect = new DoubleTurn_Effect(targetPawn.TurnInfo, effectIcon);
        doubleTurn_Effect.ApplyEffect();

        //targetPawn.AddEffectIcon(effectIcon, "blueBuff");

        //BattleLogVerticalGroup.Instance.AddPsionEntry(targetPawn.Name, PsionActionSymbol.Blue, Color.blue);

        base.OnButtonClick();
    }
}
