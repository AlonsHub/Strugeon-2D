using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBuff : SkillButton
{
    private Pawn targetPawn;
    public float percentHeal;

    //DONE!
    public override void OnButtonClick()
    {
        targetPawn = MouseBehaviour.hitTarget;
        int healAmount = (int) (((float)targetPawn.maxHP / 100f) * percentHeal);
        targetPawn.Heal(healAmount);
        //targetPawn.HP = Mathf.Clamp(targetPawn.HP ,0, targetPawn.maxHP);

        base.OnButtonClick();
    }

}