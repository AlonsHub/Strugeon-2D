using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButtonHostile : SkillButton
{
    Pawn targetPawn;
    
    //GameObject effectObject; //in character's list

    public override void OnButtonClick()
    {
        //Lose 1 turn
        targetPawn = MouseBehaviour.hitTarget;
        //if (targetPawn.DoSkipTurn)
        //{
        //    return;
        //}

        //targetPawn.DoSkipTurn = true;
        targetPawn.AddEffectIcon(effectIcon, "redDeBuff");

        //targetPawn.doSkipTurn = true;
        //targetPawn.ApplySpecialEffect(effectIcon, "Red");
        //    targetCharacter.StartCoroutine("IconDestroyer", targetCharacter.DoSkipTurn(), );
        //StartCoroutine("EndWhen");
        SkipTurn_Effect skipTurn_Effect = new SkipTurn_Effect(targetPawn.TurnInfo);
        skipTurn_Effect.ApplyEffect();

        BattleLogVerticalGroup.Instance.AddPsionEntry(targetPawn.Name, PsionActionSymbol.Red, Color.red);


        base.OnButtonClick();
    }

    //IEnumerator EndWhen()
    //{
    //    yield return new WaitUntil(() => !targetCharacter.doSkipTurn);
    //    targetCharacter.specialEffectIndicatorList.Remove(effectObject);
    //    Destroy(effectObject);

    //}
}
