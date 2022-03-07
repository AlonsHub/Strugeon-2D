using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : StatusEffectComponent, I_Attackable
{
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
    public void TakeDamage(int damage)
    {
        ReduceTtlBy(damage);
    }
}
