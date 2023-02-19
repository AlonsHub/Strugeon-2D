using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurryTurn_Effect : TurnInfoEffect
{
    public HurryTurn_Effect(TurnInfo ti) : base(ti)
    {
    }

    public override void ApplyEffect()
    {
        BeltManipulator.Instance.MoveTurnInfoToBeNext(turnInfoToEffect);
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
