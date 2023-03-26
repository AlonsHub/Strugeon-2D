using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSpell : SpellButton
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
            if (pawnTgt.statusEffects.Where(s => s is FreezeEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is FreezeEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        FreezeEffect skipTurn_Effect = new FreezeEffect(pawnTgt, effectIcon);
        //DamageModifier damageModifier = new DamageModifier();
        FreezeDamageReduction freezeDamageReducation = new FreezeDamageReduction(pawnTgt, null, damageModifier);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Red, Color.red);

        base.OnButtonClick();
    }
}
