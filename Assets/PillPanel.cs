using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPanel : MonoBehaviour
{
    [SerializeField]
    List<NulColour> coloursToShow;

    [SerializeField, Tooltip("set in this order:Orange, Yellow, Green, Blue, Red, Purple, Black")]
    List<PillGraphSlice> noolPills;

    private void Start()
    {
        SetToPsion();
    }
    public void SetToPsion()
    {
        foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements)
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            PillGraphSlice temp = noolPills[(int)nulElement.GetNulColour];
            if (!coloursToShow.Contains(nulElement.GetNulColour))
            {
                //temp.gameObject.SetActive(false); 
                continue;
            }

            temp.gameObject.SetActive(true);

            temp.SetMeFull(nulElement);
        }
    }

}
