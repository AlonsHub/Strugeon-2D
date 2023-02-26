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

    public float maxValue; //better organize with getters and setters! TBF
    public float regenRate;
    
    public PsionNulElement(NulColour col, float max, float regen)
    {
        //elementType = t;
        nulElement = new NulElement(col);
        maxValue = max;
        nulElement.value = maxValue; //TBF default starting values
        _nulColour = col;
        regenRate = regen;
    }

    public void ModifyValue(float amount)
    {
        nulElement.value += amount;
    }

    public void Regen()
    {
        nulElement.value += regenRate;
        nulElement.value = Mathf.Clamp(value, 0, maxValue);
    }
}
