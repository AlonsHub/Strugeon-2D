using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public enum NulElementType {Red, Blue, Yellow, Purple,  };
[System.Serializable]
public struct NulElement
{
    public float value;
    //public float maxValue;
    //public string elementType; //TBF define an Enum to correlate with psion power colours //WIP now 13/06
    public NulColour nulColour;

    public NulElement(NulColour colour)
    {
        nulColour = colour;
        value = 2;
        //maxValue = 1;
    }
}
[System.Serializable]
public struct PsionNulElement
{
    NulElement nulElement;
    public float value => nulElement.value;
    public NulColour nulColour => nulElement.nulColour; //TBF define an Enum to correlate with psion power colours
    
    public float maxValue;
    public float regenRate;
    
    public PsionNulElement(NulColour col, float max, float regen)
    {
        //elementType = t;
        nulElement = new NulElement(col);
        maxValue = max;
        regenRate = regen;
    }
}
