using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBuff : SkillButton
{
    public float damageMultiplier;

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;
        WeaponItem weapon = pawnTgt.GetComponent<WeaponItem>();
        if (weapon)
        {
            if (weapon.hasRedBuff)
                return;

            weapon.hasRedBuff = true; //TBF FIX THIS! 
            //targetPawn.ApplySpecialEffect(effectIcon, "Red"); //NEED THIS
            pawnTgt.AddEffectIcon(effectIcon, "redBuff");
            BattleLogVerticalGroup.Instance.AddPsionEntry(pawnTgt.Name, PsionActionSymbol.Red, Color.red);

            base.OnButtonClick();
        }

    }
}
