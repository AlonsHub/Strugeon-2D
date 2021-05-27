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
        if (targetPawn.DoSkipTurn)
        {
            return;
        }

        targetPawn.DoSkipTurn = true;
        targetPawn.AddEffectIcon(effectIcon, "redDeBuff");

        //targetPawn.doSkipTurn = true;
        //targetPawn.ApplySpecialEffect(effectIcon, "Red");
        //    targetCharacter.StartCoroutine("IconDestroyer", targetCharacter.DoSkipTurn(), );
        //StartCoroutine("EndWhen");
        base.OnButtonClick();
    }

    //IEnumerator EndWhen()
    //{
    //    yield return new WaitUntil(() => !targetCharacter.doSkipTurn);
    //    targetCharacter.specialEffectIndicatorList.Remove(effectObject);
    //    Destroy(effectObject);

    //}
}
