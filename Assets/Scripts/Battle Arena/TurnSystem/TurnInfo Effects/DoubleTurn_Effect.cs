using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurn_Effect : TurnInfoEffect
{
    public DoubleTurn_Effect(TurnInfo ti) : base(ti)
    {

    }

    public override void ApplyEffect()
    {
        //apply effect and...
        turnInfoToEffect.GetTurnTaker.DoDoubleTurn = true;
        Debug.LogError("apply double");
        //ADD Visual Elements if relevant
        // TBA

        //... subscribe to it's removal (on this characters next end-turn)
        //turnInfoToEffect.OnTurnEnd += () => EndEffect();

    }

    public override void EndEffect()
    {
        //REMOVE Visual Elements if relevant
        // TBA

        //turnInfoToEffect.OnTurnEnd -= () => EndEffect();


    }
}
