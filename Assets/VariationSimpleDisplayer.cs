using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VariationSimpleDisplayer : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    Image bg;
    
    public void SetMe(ActionVariation av, Color col)
    {
        text.text = $"{av.relevantItem} on {av.target.name}: {av.weight}";
        bg.color = col;
    }
}
