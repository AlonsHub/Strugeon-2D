using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurn_Effect : TurnInfoEffect, I_StatusEffect_TurnEnd
{
    int count = 1;
    public SkipTurn_Effect(TurnInfo ti) : base(ti)
    {
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        
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
        count--;
        if(count ==0)
        {
            EndEffect();
        }
    }
}
