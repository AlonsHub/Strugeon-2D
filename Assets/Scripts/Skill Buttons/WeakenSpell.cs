using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WeakenSpell : SkillButton
{
    [SerializeField]
    DamageModifier damageModifier;
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
       
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is WeakenEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is WeakenEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        WeakenEffect weakenEffect = new WeakenEffect(pawnTgt, effectIcon, damageModifier);

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
