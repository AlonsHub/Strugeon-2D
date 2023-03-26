using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect, I_StatusEffect_TurnStart
{
    int count = 1;

    TurnInfo turnInfoToEffect;

    public FreezeEffect(Pawn tgt, Sprite s) : base(tgt,s)
    {
        alignment = EffectAlignment.Negative;

        turnInfoToEffect = tgt.TurnInfo;
        pawnToEffect = tgt;
        
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        turnInfoToEffect.colour = SturgeonColours.Instance.skipGrey;
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, SturgeonColours.Instance.skipGrey);

        pawnToEffect.AddStatusEffect(this);
    }
    public override void EndEffect()
    {
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, Color.white);
        turnInfoToEffect.colour = new Color(0,0,0,0);

        pawnToEffect.RemoveStatusEffect(this);
    }
    public override void Perform()
    {
        pawnToEffect.FinishAnimation(); //skipping "Finish animation by directly setting to turndone to be true
        EndEffect();
    }
}
