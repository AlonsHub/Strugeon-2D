using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InsightSpell : SpellButton
{
    [SerializeField]
    GameObject insightDisplayPrefab;

    [SerializeField]
    int duration = 1;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is InsightEffect).Any())
            {
                //StatusEffect se = pawnTgt.statusEffects.Where(s => s is InsightEffect).SingleOrDefault();
                //se.StackMe();

                //shouldn't ever really happen
                return;
            }
        }
        GameObject go = Instantiate(insightDisplayPrefab, pawnTgt.transform);
        new InsightEffect(pawnTgt, effectIcon, go.GetComponent<InsightDisplay>(), duration);
        base.OnButtonClick();
    }

    public override void InteractableCheck()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        base.InteractableCheck();
        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is InsightEffect).Any())
            {
                SetButtonInteractability(false);
            }
        }
    }
}
