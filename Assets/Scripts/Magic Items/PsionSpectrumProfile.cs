using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class PsionSpectrumProfile 
{
    public List<PsionNulElement> psionElements;
    public PsionNulElement GetElementByName(string s) => psionElements.Where(x => x.elementType == s).SingleOrDefault();
    public float GetValueByName(string s) => GetElementByName(s).value;
    public float GetMaxValueByName(string s) => GetElementByName(s).maxValue;
    public PsionSpectrumProfile()
    {
        psionElements = new List<PsionNulElement>() { new PsionNulElement("Red", 1), new PsionNulElement("Blue", 1), new PsionNulElement("Yellow",1), new PsionNulElement("Purple",1) };
    }
}
