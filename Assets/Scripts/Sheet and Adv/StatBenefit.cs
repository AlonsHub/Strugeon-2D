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
        return $"{statToBenefit.ToString()} +{value}";
    }

    public StatToBenefit StatToBenefit()
    {
        return statToBenefit;
    }

    public int Value() => value;
    

    
}
