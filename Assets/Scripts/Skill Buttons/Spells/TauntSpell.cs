using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TauntSpell : SpellButton
{
    public override void OnButtonClick()
    {
        pawnTgt = MouseBehaviour.hitTarget;

        foreach (var item in RefMaster.Instance.enemyInstances)
        {
            new TauntedEffect(item, effectIcon, pawnTgt);
        }
        
        base.OnButtonClick();
    }
}