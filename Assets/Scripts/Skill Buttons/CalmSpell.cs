﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CalmSpell : SkillButton
{
    //Pawn targetPawn;

   
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        if (pawnTgt.statusEffects.Where(s => s is CalmEffect).Any())
        {
            Debug.LogError($"Spell failed! This target already has {this.GetType()} in effect.");
            return;
        }

        new CalmEffect(pawnTgt, effectIcon);
       

        base.OnButtonClick();
    }
}
