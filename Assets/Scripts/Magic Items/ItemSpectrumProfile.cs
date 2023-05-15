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
        for (int i = 0; i < System.Enum.GetValues(typeof(NulColour)).Length; i++)
        {
            elements.Add(new NulElement((NulColour)i));
        }
    }

    //public void SetAllElements(float[] valuesInOrder)
    //{
    //    for (int i = 0; i < valuesInOrder.Length; i++)
    //    {
    //        elements[i].value = valuesInOrder[i];
    //    }
    //}
    //The spectrumProfile holds the values of rbyp like a vector
}
