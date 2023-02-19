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
        List<TurnInfo> infos = TurnMachine.Instance.GetTurnInfos();
        int index = infos.FindIndex(x => x == turnInfoToEffect);

        SelfDestructing_TurnInfo newTurnInfo = new SelfDestructing_TurnInfo(turnInfoToEffect.GetTurnTaker,1);
        
        TurnMachine.Instance.InsertTurnInfo(newTurnInfo, index);

        Debug.LogError("apply double");
       
    }

    public override void EndEffect()
    {
        //REMOVE Visual Elements if relevant
        // TBA
        (turnInfoToEffect.GetTurnTaker as Pawn).RemoveIconByName("blueBuff"); //BAD! CHANGE THIS!
        turnInfoToEffect.DoDoubleTurn = false;
        //turnInfoToEffect.OnTurnEnd -= () => EndEffect();
    }

    public override void Perform()
    {
        Debug.LogError("This shouldn't really be performed");
    }
}
