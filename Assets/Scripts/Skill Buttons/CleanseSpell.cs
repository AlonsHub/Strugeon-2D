using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CleanseSpell : SkillButton
{
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            StatusEffect[] se = pawnTgt.statusEffects.Where(s => s.alignment == EffectAlignment.Negative).ToArray();
            if (se.Length != 0)
            {
                foreach (var item in se)
                {
                    item.EndEffect();
                }
                
            }
        }

        base.OnButtonClick();
    }
}
