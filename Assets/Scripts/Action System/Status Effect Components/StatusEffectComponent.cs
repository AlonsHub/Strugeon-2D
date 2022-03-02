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


    /// <summary>
    /// For "below HPBar display"
    /// </summary>
    Sprite effectIcon;
    int ttl;
    int maxLifetime;
    Pawn tgtPawn;

    public void SetMe(Pawn target, string buffIconName)
    {
        iconName = buffIconName;
        effectIcon = Resources.Load<Sprite>($"Icons/{iconName}"); //could be loaded elsewhere/beforehand?
        tgtPawn = target;
        //apply the buff to target

        StartCoroutine(TurnCounter());
    }

    IEnumerator TurnCounter()
    {
        tgtPawn.AddEffectIcon(effectIcon, iconName);
        while (ttl > 0)
        {
            yield return new WaitUntil(() => tgtPawn.TurnDone);
            ttl--;
            yield return new WaitUntil(() => !tgtPawn.TurnDone); //I THINK THIS CAN BE REMOVED!
        }
        tgtPawn.RemoveIconByColor(iconName);
        
        //undo the buff
        
        Destroy(this); //just this Blinded component! 
    }
    /// <summary>
    /// current life = remaining 
    /// </summary>
    public int currentTtl => ttl;
}
