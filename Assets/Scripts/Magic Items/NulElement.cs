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
        value = 0f;
        //maxValue = 1;
    }
}
[System.Serializable]
public class PsionNulElement
{
    NulElement nulElement;
    public float value => nulElement.value;
    public NulColour GetNulColour => nulElement.nulColour; //TBF define an Enum to correlate with psion power colours

    [SerializeField]
    NulColour _nulColour; //this needs to get set once on creation

    public float maxValue;
    public float regenRate;
    
    public PsionNulElement(NulColour col, float max, float regen)
    {
        //elementType = t;
        nulElement = new NulElement(col);
        nulElement.value = .5f; //TBF default starting values
        _nulColour = col;
        maxValue = max;
        regenRate = regen;
    }
}
