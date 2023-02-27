using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowButtonHostile : SkillButton
{
    //Pawn targetPawn;
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
        pawnTgt = MouseBehaviour.hitTarget;
        if (pawnTgt.DoYellowDebuff)
        {
            return;
        }
        pawnTgt.DamageModifier = dmgMultiplier;
        pawnTgt.DoYellowDebuff = true;

        pawnTgt.AddEffectIcon(effectIcon, "yellowDeBuff");

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Yellow, Color.yellow);


        base.OnButtonClick();
    }

    //IEnumerator EndWhen()
    //{
    //    yield return new WaitUntil(() => !targetCharacter.doTakeSkillDamage);
    //    targetCharacter.specialEffectIndicatorList.Remove(effectObject);
    //    Destroy(effectObject);
    //}
}
