using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTurn_Effect : StatusEffect, I_StatusEffect_TurnEnd
{
    TurnInfo newTurnInfo;
    int count;

    TurnInfo turnInfoToEffect;

    public DoubleTurn_Effect(Pawn tgt, Sprite s) : base(tgt, s)
    {
        alignment = EffectAlignment.Positive;
        turnInfoToEffect = tgt.TurnInfo;

        count = 0;
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        int index;
        List<TurnInfo> infos = TurnMachine.Instance.GetTurnInfos();

        if (infos.Contains(turnInfoToEffect))
        {
            index = infos.FindIndex(x => x == turnInfoToEffect);
        }
        else
        {
            Debug.LogError("Does not contain");
            return;
        }

        newTurnInfo = new TurnInfo(turnInfoToEffect.GetTurnTaker);
        
        TurnMachine.Instance.InsertTurnInfo(newTurnInfo, index);
        pawnToEffect.AddStatusEffect(this);
    }
    public override void EndEffect()
    {
        TurnMachine.Instance.RemoveTurnInfo(newTurnInfo);
        pawnToEffect.RemoveStatusEffect(this);

        //base.EndEffect();
    }

    public override void Perform()
    {
        //more than double turn (x3 and up) can be done here with a duration instead
        count++;
        if(count==2)
        {
            EndEffect();
        }
    }
}