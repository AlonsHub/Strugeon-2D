using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatToBenefit { MaxHP, FlatDamage };
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
            case global::StatToBenefit.FlatDamage:
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
            case global::StatToBenefit.FlatDamage:
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
