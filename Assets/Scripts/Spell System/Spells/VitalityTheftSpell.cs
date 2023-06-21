using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VitalityTheftSpell : SpellButton
{
    [SerializeField]
    GameObject specialColliderPrefab;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is MarkedEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is MarkedEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        SpecialCollider sc = Instantiate(specialColliderPrefab,pawnTgt.transform).GetComponent<SpecialCollider>();
        float dmg = pawnTgt.currentHP *(modifier * 0.2f /100); //as percentage of currentHealth 


        sc.Init((int)dmg, pawnTgt);
        new VitalityTheftEffect(pawnTgt, effectIcon, dmg, sc);

        base.OnButtonClick();
    }
}
