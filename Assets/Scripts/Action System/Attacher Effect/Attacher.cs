using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : StatusEffectComponent, I_Attackable
{
    protected int attacherHP;
    //SetMe need not be overwritten, just use as is
    //public override void ApplyEffect()
    //{
    //    base.ApplyEffect();
    //    //override pawns TakeDamage function
    //}
    //public override void RemoveEffect()
    //{
    //    base.RemoveEffect();
    //    //enable pawns TakeDamage function to normal
    //}
    public int TakeDamage(int damage)
    {
        //ReduceTtlBy(damage);
        attacherHP -= damage;
        if(attacherHP<=0)
        {
            RemoveEffect();
        }
        return attacherHP;
    }
}
