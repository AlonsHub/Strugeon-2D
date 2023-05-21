using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkSpell : SpellButton
{

    [SerializeField]
    int missChance;
    [SerializeField]
    DamageModifier damageModifier;
    [SerializeField]
    DamageModifier dodgeDamageModifier;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ShrinkEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is ShrinkEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        damageModifier.mathOperator = MathOperator.SubtractPercentage;
        damageModifier.mod = (35 - modifier * .05f);
        Debug.Log($"Shrink redueces damage by {damageModifier.mod} perect");
        new ShrinkEffect(pawnTgt, effectIcon, damageModifier);

        missChance = (int) (modifier * .13f / 1.1f);
        new Shrink_DodgeEffect(pawnTgt, null, dodgeDamageModifier, missChance);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Blue, SturgeonColours.Instance.noolBlue);

        base.OnButtonClick();
    }
}
