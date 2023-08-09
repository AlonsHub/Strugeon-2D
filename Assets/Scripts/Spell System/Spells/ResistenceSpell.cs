using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResistenceSpell : SpellButton
{
    /// <summary>
    /// This is where the stats are pulled form -> the serialized field in the spells components
    /// </summary>
    [SerializeField]
    DamageModifier damageModifier;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is ResistenceEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is ResistenceEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        damageModifier.mod = modifier * 0.05f;
        //ResistenceEffect skipTurn_Effect = new ResistenceEffect(pawnTgt, effectIcon, damageModifier);
        //DamageModifier damageModifier = new DamageModifier();
        ResistenceEffect resistanceEffect = new ResistenceEffect(pawnTgt, effectIcon, damageModifier);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Yellow, SturgeonColours.Instance.noolYellow);

        base.OnButtonClick();
    }
}
