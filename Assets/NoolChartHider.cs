using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoolChartHider : MonoBehaviour
{
    [SerializeField, Tooltip("Set by colour order")]
    Image[] hiders;


    
    void Start()
    {
        SetAllHidersToValue(0);
    }

    public void SetAllHidersToValue(float allVlaue)
    {
        foreach (var item in hiders)
        {
            item.fillAmount = allVlaue;
        }
    }

    public void SetHiderToValue(NoolColour col, float newValue)
    {
        hiders[(int)col].fillAmount = newValue;
    }
    public void SetHiderToValue(int col, float newValue)
    {
        hiders[col].fillAmount = newValue;
    }
  
}
