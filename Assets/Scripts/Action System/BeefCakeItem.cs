using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeefCakeItem : MonoBehaviour, SA_Item
{
    Pawn pawn;

    [SerializeField]
    float hpThresholdPercent;
    [SerializeField]
    float damageModifier;
    [SerializeField]
    Sprite beefCakeSprite;
    public bool SA_Available()
    {
        return IsThreshold();
    }

    public string SA_Description()
    {
        return $"When {pawn.Name}'s HP gets below {hpThresholdPercent}%, they take x{damageModifier} damage.";
    }

    public string SA_Name()
    {
        return "Beef Cake";
    }

    public Sprite SA_Sprite()
    {
        return beefCakeSprite;
    }

    public void SetToLevel(int level)
    {
        //currently un-implemented
        //needs gamedesign 
        //TBF

        //throw new System.NotImplementedException();
    }

    public void StartCooldown()
    {
        //irrelevant
    }

    void Start()
    {
        pawn = GetComponent<Pawn>();

        pawn.OnTakeDamage += TryEffect;
    }

    void TryEffect()
    {
        if(IsThreshold())
        {
            pawn.FinalDamageMod = damageModifier;
            //add effect VFX
        }
        else
        {
            pawn.FinalDamageMod = 1f;
        }
    }

    bool IsThreshold()
    {
        return (float)((float)pawn.currentHP / (float)pawn.maxHP) <= (float)hpThresholdPercent /100f;
    }

    
}
