using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TauntSpell : SkillButton
{

    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        //if (pawnTgt.statusEffects.Where(s => s is TauntedEffect).Any())
        //{
        //    Debug.LogError($"Spell failed! This target already has {this.GetType()} in effect.");
        //    return;
        //}

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new TauntedEffect(item, effectIcon, pawnTgt);
        }
        
        base.OnButtonClick();
    }
}