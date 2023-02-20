using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Good for DoubleTurn type effects, but also temporary elements on the field - such as hazards or effects which may have a timed existence
/// </summary>
public class SelfDestructing_TurnInfo : TurnInfo
{
    int currentTtl;
    public SelfDestructing_TurnInfo(TurnTaker t, int ttl) : base(t)
    {
        currentTtl = ttl;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();
        currentTtl--;
        if (currentTtl < 0)
            TurnMachine.Instance.RemoveTurnInfo(this);
    }
}
