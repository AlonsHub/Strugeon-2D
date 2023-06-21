using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BlindRageSpell : SpellButton
{
    //Pawn targetPawn;
    [SerializeField]
    DamageModifier damageModifier;

    public override void OnButtonClick()
    {
        damageModifier.mod = modifier * .01f * (14 / 100);

        pawnTgt = MouseBehaviour.hitTarget;
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is BlindRageEffect).Any())
            {
                Debug.LogError($"Spell failed! This target already has {this.GetType()} in effect.");
                return;
            }
        }
        
        new BlindRageEffect(pawnTgt, effectIcon, damageModifier);

        //new DamageRelatedStatusEffect();

        base.OnButtonClick();
    }

}
