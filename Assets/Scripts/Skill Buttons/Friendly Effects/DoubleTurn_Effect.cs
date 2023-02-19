using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurn_Effect : TurnInfoEffect
{
    public DoubleTurn_Effect(TurnInfo ti, Sprite s) : base(ti,s)
    {
    }

    public override void ApplyEffect()
    {
        List<TurnInfo> infos = TurnMachine.Instance.GetTurnInfos();
        int index = infos.FindIndex(x => x == turnInfoToEffect);

        SelfDestructing_TurnInfo newTurnInfo = new SelfDestructing_TurnInfo(turnInfoToEffect.GetTurnTaker,1);
        
        TurnMachine.Instance.InsertTurnInfo(newTurnInfo, index);

        base.ApplyEffect();
    }

    public override void Perform()
    {
        Debug.LogError("This shouldn't really be performed");
    }
}