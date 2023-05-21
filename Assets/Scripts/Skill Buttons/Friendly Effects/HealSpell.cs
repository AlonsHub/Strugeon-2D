using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : SpellButton
{
    //private Pawn targetPawn;
    public float percentHeal;

    //DONE!
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        //int healAmount = (int) (((float)pawnTgt.maxHP / 100f) * percentHeal);

        //TEMP! TBD! TBF! need to make Heal an effect 
        int healAmount = (int) (pawnTgt.maxHP/(modifier *0.1f));
        pawnTgt.Heal(healAmount);
        //targetPawn.HP = Mathf.Clamp(targetPawn.HP ,0, targetPawn.maxHP);

        BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Yellow, Color.yellow);


        base.OnButtonClick();
    }

}