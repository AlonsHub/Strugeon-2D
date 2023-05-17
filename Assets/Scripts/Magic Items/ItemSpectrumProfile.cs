using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemSpectrumProfile 
{
    //public float red, blue, yellow, purple;
    public List<NulElement> elements;

    public ItemSpectrumProfile()
    {
        elements = new List<NulElement>();// {new NulElement("Red"), new NulElement("Blue"), new NulElement("Yellow"), new NulElement("Purple")};
        for (int i = 0; i < System.Enum.GetValues(typeof(NoolColour)).Length; i++)
        {
            elements.Add(new NulElement((NoolColour)i));
        }
    }
}
