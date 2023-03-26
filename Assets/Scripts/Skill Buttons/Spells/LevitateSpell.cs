using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevitateSpell : SpellButton
{
    [SerializeField]
    int duration;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;


        if (pawnTgt.statusEffects != null && pawnTgt.statusEffects.Count != 0)
        {
            if (pawnTgt.statusEffects.Where(s => s is RootDownEffect).Any())
            {
                Debug.LogError("Can't Levitate due to RootDown effect");
                return;
            }


            if (pawnTgt.statusEffects.Where(s => s is LevitateEffect).Any())
            {
                StatusEffect se = pawnTgt.statusEffects.Where(s => s is LevitateEffect).SingleOrDefault();
                se.StackMe(); //This may be more relevant if freeze can be improved to have more duration
                return;
            }
        }

        new LevitateEffect(pawnTgt, effectIcon, duration);
        

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Blue, SturgeonColours.Instance.noolBlue);

        base.OnButtonClick();
    }
}
