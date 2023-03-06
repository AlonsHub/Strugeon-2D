using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : TurnInfoEffect, I_StatusEffect_TurnStart
{
    int count = 1;
    public FreezeEffect(TurnInfo ti, Sprite s) : base(ti,s)
    {
        alignment = EffectAlignment.Negative;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        turnInfoToEffect.colour = SturgeonColours.Instance.skipGrey;
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, SturgeonColours.Instance.skipGrey);

        base.ApplyEffect();
    }
    public override void EndEffect()
    {
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, Color.white);
        turnInfoToEffect.colour = new Color(0,0,0,0);

        base.EndEffect();
    }
    public override void Perform()
    {
        turnInfoToEffect.TryGetPawn().TurnDone = true; //skipping "Finish animation by directly setting to turndone to be true
        EndEffect();
    }
}
