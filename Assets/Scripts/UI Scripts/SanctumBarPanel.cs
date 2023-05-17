using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumBarPanel : MonoBehaviour
{
    public static SanctumBarPanel Instance;

    [SerializeField]
    List<NoolColour> coloursToShow;

    [SerializeField, Tooltip("Set in this order: Orange, Yellow, Green, Blue, Red, Purple, Black")]
    List<NulBar> nulBars;

    [SerializeField]
    MagicItemSO emptyItem;

    private void Awake()
    {
        Instance = this; //TBD TBF LAZY
        //if (setOnAwake)
            SetMe(emptyItem.magicItem.spectrumProfile);
    }

   

    /// <summary>
    /// Uses item stats, Max values should still matter? do we still use the player's stats for that?
    /// </summary>
    public void SetMe(ItemSpectrumProfile itemSpectrumProfile)
    {
        foreach (var nulElement in itemSpectrumProfile.elements)
        {
            nulBars[(int)nulElement.nulColour].SetMe(nulElement);
        }
    }

}
