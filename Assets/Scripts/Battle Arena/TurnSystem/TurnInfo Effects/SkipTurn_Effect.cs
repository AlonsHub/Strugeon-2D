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
        turnInfoToEffect.DoSkipTurn = true;
        turnInfoToEffect.OnTurnSkip += ()=> EndEffect();
        //Add effect icon -> that will also be destoryed with the effect ending
    }
    public override void EndEffect()
    {
        turnInfoToEffect.DoSkipTurn = false;
        turnInfoToEffect.OnTurnSkip -= () => EndEffect();
        //Remove effect icon -> that will also be destoryed with the effect ending
    }
}
