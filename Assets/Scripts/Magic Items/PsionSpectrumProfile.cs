using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class PsionSpectrumProfile 
{
    public List<PsionNulElement> psionElements;
    public PsionNulElement GetElementByName(NulColour s) => psionElements.Where(x => x.GetNulColour == s).SingleOrDefault();
    public float GetValueByName(NulColour s) => GetElementByName(s).value;
    public float GetMaxValueByName(NulColour s) => GetElementByName(s).maxValue;

    public System.Action OnAnyValueChanged;
    public PsionSpectrumProfile()
    {
        psionElements = new List<PsionNulElement>();// { new PsionNulElement("Red", 1), new PsionNulElement("Blue", 1), new PsionNulElement("Yellow",1), new PsionNulElement("Purple",1) };
        for (int i = 0; i < System.Enum.GetNames(typeof(NulColour)).Length; i++)
        {
            psionElements.Add(new PsionNulElement((NulColour)i, 1f, .2f)); //starting values!
        }
    }

    public int SumOfAllCapacities()
    {
        float sum = 0;
        foreach (var item in psionElements)
        {
            sum += item.maxValue;
        }
        return Mathf.FloorToInt(sum);
    }

    public void IncreaseMaxValue(NulColour s, float amount)
    {
        psionElements[(int)s].maxValue += amount; //TBF should also be modify value
        OnAnyValueChanged?.Invoke();
    }

    public void ModifyCurrentValue(NulColour s, float amount)
    {
        psionElements[(int)s].ModifyValue(amount);
        OnAnyValueChanged?.Invoke();
    }


    public void PerformRegenAll()
    {
        foreach (var item in psionElements)
        {
            item.Regen();
        }
        OnAnyValueChanged?.Invoke();
    }
}
