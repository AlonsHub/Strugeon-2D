using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowButtonHostile : SkillButton
{
    Pawn targetPawn;
    [SerializeField]
    float dmgMultiplier;
    //ameObject effectObject;
    public override void OnButtonClick()
    {
        //x1.5 dmg
       // targetPawn = MouseBehaviour.hitTarget;
        //targetPawn.doTakeSkillDamage = true;
        //targetPawn.skillDamageMultiplier = dmgMultiplier;
        //effectIconIndex = targetCharacter.ApplySpecialEffect(effectIcon);
        //targetPawn.ApplySpecialEffect(effectIcon, "Yellow");
        //StartCoroutine("EndWhen");
        targetPawn = MouseBehaviour.hitTarget;
        if (targetPawn.DoModifyDamage)
        {
            return;
        }
        targetPawn.DamageModifier = dmgMultiplier;
        targetPawn.DoModifyDamage = true;
        // targetPawn.ApplySpecialEffect(effectIcon, "Blue");
        targetPawn.AddEffectIcon(effectIcon, "yellowDeBuff");
        base.OnButtonClick();
    }

    //IEnumerator EndWhen()
    //{
    //    yield return new WaitUntil(() => !targetCharacter.doTakeSkillDamage);
    //    targetCharacter.specialEffectIndicatorList.Remove(effectObject);
    //    Destroy(effectObject);
    //}
}
