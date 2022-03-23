using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectComponent : MonoBehaviour
{
    /// <summary>
    /// needed to add to the dir of Resources.Load<Sprite>($"Icons/{iconName}")
    /// </summary>
    [SerializeField]
    string iconName;

    //public access
    public int currentTtl => ttl;

    //local  

    /// <summary>
    /// For "below HPBar display"
    /// </summary>
    Sprite effectIcon;
    protected int ttl;
    protected int maxTTL;
    protected Pawn tgtPawn;

    public virtual void SetMe(Pawn target, string buffIconName, int newMaxTTL)
    {
        iconName = buffIconName;
        effectIcon = Resources.Load<Sprite>($"Icons/{iconName}"); //could be loaded elsewhere/beforehand?
        tgtPawn = target;
        ttl = maxTTL = newMaxTTL;
    }
    /// <summary>
    /// Must be overridden, and must use base.ApplyEffect() to addEffectIcon to the pawns hp bar
    /// </summary>
    public virtual void ApplyEffect() //must be overridden, and must use base
    {
        tgtPawn.AddEffectIcon(effectIcon, iconName);
    }
    /// <summary>
    /// Must be overridden, and must use base.RemoveEffect() to removeIconByColor to the pawns hp bar
    /// </summary>
    public virtual void RemoveEffect()
    {
        tgtPawn.RemoveIconByName(iconName);
        Destroy(this);
    }
    public virtual int ReduceTtlBy(int reduceBy)
    {
        ttl -= reduceBy;
        if (ttl <= 0)
        {
            RemoveEffect();
        }
        return ttl;
    }

    protected void SetTTLtoMax()
    {
        ttl = maxTTL;
    }

    public virtual void ReduceTtlByOne()
    {
        ttl--;
        if (ttl <= 0)
        {
            RemoveEffect();
        }
    }




}
