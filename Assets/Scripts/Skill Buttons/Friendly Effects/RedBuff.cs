using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBuff : SkillButton
{
    private Pawn targetPawn;
    public float damageMultiplier;

    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;
        WeaponItem weapon = targetPawn.GetComponent<WeaponItem>();
        if (weapon)
        {
            weapon.hasRedBuff = true;
            //targetPawn.ApplySpecialEffect(effectIcon, "Red"); //NEED THIS
        }
        targetPawn.AddEffectIcon(effectIcon, "redBuff");

        BattleLogVerticalGroup.Instance.AddPsionEntry(targetPawn.Name, PsionActionSymbol.Red, Color.red);

        base.OnButtonClick();
    }
}
