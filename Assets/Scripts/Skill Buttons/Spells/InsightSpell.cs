using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InsightSpell : SpellButton
{
    [SerializeField]
    GameObject insightDisplayPrefab;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is InsightEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is InsightEffect).SingleOrDefault();
                se.StackMe();
                return;
            }
        }
        GameObject go = Instantiate(insightDisplayPrefab, pawnTgt.transform);
        new InsightEffect(pawnTgt, effectIcon, go.GetComponent<InsightDisplay>());
        base.OnButtonClick();
    }
}
