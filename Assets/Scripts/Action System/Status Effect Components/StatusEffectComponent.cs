using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectComponent : MonoBehaviour
{
    //Data to set in inspector:

    /// <summary>
    /// needed to add to the dir of Resources.Load<Sprite>($"Icons/{iconName}")
    /// </summary>
    [SerializeField]
    string iconName;
    [SerializeField]
    int maxLifetime;

    
    //local shit 

    /// <summary>
    /// For "below HPBar display"
    /// </summary>
    Sprite effectIcon;
    protected int ttl;
    protected Pawn tgtPawn;

    public virtual void ApplyEffect()
    {
        tgtPawn.AddEffectIcon(effectIcon, iconName);
    }
    public virtual void RemoveEffect()
    {
        tgtPawn.RemoveIconByColor(iconName);
        Destroy(this);
    }
     public virtual void ReduceTtlBy(int reduceBy)
    {
        ttl -= reduceBy;
        if(ttl <= 0)
        {
            RemoveEffect();
        }
    }

public virtual void ReduceTtlByOne()
    {
        ttl --;
        if(ttl <= 0)
        {
            RemoveEffect();
        }
    }


    public virtual void SetMe(Pawn target, string buffIconName)
    {
        iconName = buffIconName;
        effectIcon = Resources.Load<Sprite>($"Icons/{iconName}"); //could be loaded elsewhere/beforehand?
        tgtPawn = target;
        //apply the buff to target
        //StartCoroutine(TurnCounter());
    }

    //public virtual IEnumerator TurnCounter()
    //{
    //    tgtPawn.AddEffectIcon(effectIcon, iconName);
    //    ApplyEffect();

    //    while (ttl > 0)
    //    {
    //        yield return new WaitUntil(() => tgtPawn.TurnDone);
    //        ttl--;
    //        yield return new WaitUntil(() => !tgtPawn.TurnDone); //I THINK THIS CAN BE REMOVED!
    //    }

    //    tgtPawn.RemoveIconByColor(iconName);
    //    RemoveEffect();


    //    Destroy(this); //just this Blinded component! 
    //}
    /// <summary>
    /// current life = remaining 
    /// </summary>
    public int currentTtl => ttl;
}
