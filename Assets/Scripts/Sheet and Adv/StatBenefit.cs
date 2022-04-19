using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatToBenefit { MaxHP, MinDamage, MaxDamage, BothDamage };
[System.Serializable]
public class StatBenefit : IBenefit
{
    public StatToBenefit statToBenefit;
    [SerializeField]
     int value;

    public string BenefitProperNoun()
    {
        switch (statToBenefit)
        {
            case global::StatToBenefit.MaxHP:
                return "Sturdiness";
                break;
            case global::StatToBenefit.MinDamage:
                return "Courage"; //Approved by 2 witness - also suggested "enthusiasem"
                break;
            case global::StatToBenefit.MaxDamage:
                return "Brutality";
                break;
            case global::StatToBenefit.BothDamage:
                return "Power";
                break;
            default:
                return "Nothing";
                break;
        }
    }

    public string BenefitStatName()
    {
        switch (statToBenefit)
        {
            case global::StatToBenefit.MaxHP:
                return "Maximum HP";
                break;
            case global::StatToBenefit.MinDamage:
                return "Minimum Damage"; //Approved by 2 witness - also suggested "enthusiasem"
                break;
            case global::StatToBenefit.MaxDamage:
                return "Maximum Damage";
                break;
            case global::StatToBenefit.BothDamage:
                return "Maximum and Minimum Damage";
                break;
            default:
                return "Nothing";
                break;
        }
    }

    public StatToBenefit StatToBenefit()
    {
        return statToBenefit;
    }

    public int Value() => value;
    

    
}
