﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurn_Effect : TurnInfoEffect, I_StatusEffect_TurnEnd
{
    TurnInfo newTurnInfo;
    int count;
    public DoubleTurn_Effect(TurnInfo ti, Sprite s) : base(ti,s)
    {
        ApplyEffect();
        count = 0;
    }

    public override void ApplyEffect()
    {
        List<TurnInfo> infos = TurnMachine.Instance.GetTurnInfos();
        int index = infos.FindIndex(x => x == turnInfoToEffect);

        //SelfDestructing_TurnInfo newTurnInfo = new SelfDestructing_TurnInfo(turnInfoToEffect.GetTurnTaker,1);
        newTurnInfo = new TurnInfo(turnInfoToEffect.GetTurnTaker);
        
        TurnMachine.Instance.InsertTurnInfo(newTurnInfo, index);

        base.ApplyEffect();
    }
    public override void EndEffect()
    {
        TurnMachine.Instance.RemoveTurnInfo(newTurnInfo);

        base.EndEffect();
    }

    public override void Perform()
    {
        //more than double turn (x3 and up) can be done here with a duration instead
        count++;
        if(count ==2)
        {
            EndEffect();
        }

    }
}