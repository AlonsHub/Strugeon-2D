using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public enum NulElementType {Red, Blue, Yellow, Purple,  };
[System.Serializable]
public struct NulElement
{
    public float value;
    //public float maxValue;
    public string elementType; //TBF define an Enum to correlate with psion power colours

    public NulElement(string t)
    {
        elementType = t;
        value = 0;
        //maxValue = 1;
    }
}
[System.Serializable]
public struct PsionNulElement
{
    NulElement nulElement;
    public float value => nulElement.value;
    public string elementType => nulElement.elementType; //TBF define an Enum to correlate with psion power colours
    
    public float maxValue;
    
    public PsionNulElement(string t, int max)
    {
        //elementType = t;
        nulElement = new NulElement(t);
        maxValue = max;
    }
}
