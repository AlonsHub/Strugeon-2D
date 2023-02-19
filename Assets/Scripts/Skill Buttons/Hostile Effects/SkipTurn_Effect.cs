using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurn_Effect : TurnInfoEffect
{
    public SkipTurn_Effect(TurnInfo ti) : base(ti)
    {

    }

    public override void ApplyEffect()
    {
        turnInfoToEffect.AddEffect(this);
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, SturgeonColours.Instance.skipGrey);

        base.ApplyEffect();
    }
    public override void EndEffect()
    {
        BeltManipulator.Instance.SetPortraitColour(turnInfoToEffect, Color.white);

        base.EndEffect();
    }
    public override void Perform()
    {
        throw new System.NotImplementedException();
    }
}
