﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemSpectrumProfile 
{
    //public float red, blue, yellow, purple;
    public List<NulElement> elements;

    public ItemSpectrumProfile()
    {
        elements = new List<NulElement>() {new NulElement("Red"), new NulElement("Blue"), new NulElement("Yellow"), new NulElement("Purple")};
    }
    //The spectrumProfile holds the values of rbyp like a vector
}
