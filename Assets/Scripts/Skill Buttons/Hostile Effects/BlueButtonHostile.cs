using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueButtonHostile : SkillButton
{
    TileWalker targetWalker;
    [SerializeField]
    int stepLimit;
    Pawn targetPawn;
    //GameObject effectObject; //in character's list

    public override void OnButtonClick()
    {
        //Step limiter
        targetPawn = MouseBehaviour.hitTarget;
        targetWalker = targetPawn.tileWalker;
        if(targetWalker.doLimitSteps)
        {
            return;
        }

        targetPawn.AddEffectIcon(effectIcon, "blueDeBuff");
        targetWalker.doLimitSteps = true;
        targetWalker.stepLimit = stepLimit;

        //targetPawn.AddEffectIcon(effectIcon, "redBuff");
        //targetWalker.stepLimit = stepLimit;
        //targetWalker.doLimitSteps = true;
        //targetPawn.ApplySpecialEffect(effectIcon, "Blue");
        //targetCharacter.StartCoroutine("IconDestroyer");
        //StartCoroutine("EndWhen");
        BattleLogVerticalGroup.Instance.AddPsionEntry(targetPawn.Name, PsionActionSymbol.Blue, Color.blue);

        base.OnButtonClick();
    }

    //IEnumerator EndWhen()
    //{
    //    yield return new WaitUntil(() => !targetWalker.doLimitSteps);
    //    targetCharacter.specialEffectIndicatorList.Remove(effectObject);
    //    Destroy(effectObject);
    //}
}
