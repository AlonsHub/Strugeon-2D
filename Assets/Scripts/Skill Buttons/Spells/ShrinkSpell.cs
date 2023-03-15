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

        new ShrinkEffect(pawnTgt, effectIcon, damageModifier);
        new DodgeEffect(pawnTgt, null, dodgeDamageModifier, missChance);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Blue, SturgeonColours.Instance.noolBlue);

        base.OnButtonClick();
    }
}
