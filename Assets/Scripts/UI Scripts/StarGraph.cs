using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGraph : MonoBehaviour
{
    [SerializeField]
    List<NoolColour> coloursToShow;

    [SerializeField, Tooltip("set in this order:Orange, Yellow, Green, Blue, Red, Purple, Black")]
    List<StarGraphSlice> nulBars;

    //[SerializeField]
    //bool isPsionNools;
    //private void Start()
    //{
    //    if(isPsionNools) //also hook to some form of OnValueChangeded for the Nools OR!!!! refresh via ItemInhaler
    //    SetToPsion();
    //}

    //public void SetToPsion()
    //{
    //    foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.psionSpectrum.psionElements)
    //    {
    //        //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
    //        StarGraphSlice temp = nulBars[(int)nulElement.GetNulColour];
    //        if (!coloursToShow.Contains(nulElement.GetNulColour))
    //        {
    //            //temp.gameObject.SetActive(false); 
    //         continue;
    //        }

    //        temp.gameObject.SetActive(true);

    //        temp.SetMe(nulElement);
    //    }
    //}
    public void SetToItem(MagicItem item)
    {
        foreach (var nulElement in item.spectrumProfile.elements)
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            if (!coloursToShow.Contains(nulElement.nulColour))
            {
                //temp.gameObject.SetActive(false); 
             continue;
            }
            StarGraphSlice temp = nulBars[(int)nulElement.nulColour];

            temp.gameObject.SetActive(true);

            temp.SetMe(nulElement.value);
        }
    }
    public void SetToMerc(MercSheet mercSheet)
    {
        foreach (var nulElement in mercSheet.temp_SpectrumProfile.elements)
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            if (!coloursToShow.Contains(nulElement.nulColour))
            {continue;}

            StarGraphSlice temp = nulBars[(int)nulElement.nulColour];

            temp.gameObject.SetActive(true);

            temp.SetMe(nulElement.value);
        }
    }


}
