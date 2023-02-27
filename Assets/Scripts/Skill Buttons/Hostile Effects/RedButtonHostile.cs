using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButtonHostile : SkillButton
{
    //Pawn targetPawn;
    
    //GameObject effectObject; //in character's list

    public override void OnButtonClick()
    {
        //Lose 1 turn
        pawnTgt = MouseBehaviour.hitTarget;
       
        SkipTurn_Effect skipTurn_Effect = new SkipTurn_Effect(pawnTgt.TurnInfo, effectIcon);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Red, Color.red);


        base.OnButtonClick();
    }

    
}
