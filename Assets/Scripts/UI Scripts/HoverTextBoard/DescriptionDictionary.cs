using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionDictionary : MonoBehaviour
{
    Dictionary<PsionActionSymbol, string> colorToBuffDescription;
    [SerializeField]
    List<PsionActionSymbol> colorList = new List<PsionActionSymbol> { PsionActionSymbol.Red, PsionActionSymbol.Blue, PsionActionSymbol.Yellow, PsionActionSymbol.Purple };

    [SerializeField]
    List<string> buffDescriptions;

    private void Start()
    {
        
    }
}
