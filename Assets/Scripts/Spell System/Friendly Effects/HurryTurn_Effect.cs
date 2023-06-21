using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THIS ISNT REALLY A STATUS EFFECT! THATS WHY IT DISREGARDS MOST OF THE LOGIC!
/// </summary>
public class HurryTurn_Effect : TurnInfoEffect
{
    public HurryTurn_Effect(TurnInfo ti) : base(ti)
    {
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        BeltManipulator.Instance.MoveTurnInfoToBeNext(turnInfoToEffect);

        //AND DONT DO BASE! DONT ADD TO STATUS EFFECTS!
    }

    public override void EndEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void Perform()
    {
        throw new System.NotImplementedException();
    }
}
