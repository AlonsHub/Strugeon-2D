using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacher : StatusEffectComponent, I_Attackable
{
    protected int attacherHP;

    GameObject visualEffectPrefab;
    //Sprite visualAddon;

    GameObject instantiateEffect;

    public void SetMeWithVFX(Pawn target, string buffIconName, int newMaxTTL, GameObject vfx)
    {
        base.SetMe(target, buffIconName, newMaxTTL);
        visualEffectPrefab = vfx;
    }

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

    public override void RemoveEffect()
    {
        if (instantiateEffect)
            Destroy(instantiateEffect);
        base.RemoveEffect();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        if (visualEffectPrefab)
            instantiateEffect = Instantiate(visualEffectPrefab, tgtPawn.transform);
    }
}
