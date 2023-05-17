using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Pill
{
    public NoolColour colour;
    public float potential;
}
[System.Serializable]
public struct Nool
{
    public Pill relatedPill;

    public float currentValue;
    public float maxValue;
    public float regenRate;
}

[System.Serializable]
public class PillProfile 
{
    public Pill[] pills;
    public NoolColour dominant;
    //public PillProfile(float allValues)
    //{
    //    int noolsLength = System.Enum.GetValues(typeof(NulColour)).Length;
    //    pills = new Pill[noolsLength];

    //    for (int i = 0; i < noolsLength; i++)
    //    {
    //        pills[i] = 
    //    }
    //}
}

[System.Serializable]
public class NoolProfile : PillProfile //Maintain the fact that you must have a PillProfile to have Nools - even if those aren't related yet, they probably will be
{
    public Nool[] nools;

    //Some setter that affects all regen values

    //Getters
}