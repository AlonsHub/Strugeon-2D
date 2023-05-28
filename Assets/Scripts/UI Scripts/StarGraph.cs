using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGraph : MonoBehaviour
{
    [SerializeField]
    List<NoolColour> coloursToShow;

    [SerializeField, Tooltip("set in this order:Orange, Yellow, Green, Blue, Red, Purple, Black")]
    List<StarGraphSlice> nulBars;

    public void SetToItem(MagicItem item)
    {
        foreach (var pill in item.pillProfile.pills)
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            if (!coloursToShow.Contains(pill.colour))
            {
                //temp.gameObject.SetActive(false); 
             continue;
            }
            StarGraphSlice temp = nulBars[(int)pill.colour];

            temp.gameObject.SetActive(true);

            temp.SetMe(pill.potential);
        }
    }
    public void SetToValues(float[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (!coloursToShow.Contains((NoolColour)i))
            {
                continue;
            }
            StarGraphSlice temp = nulBars[i];

            temp.gameObject.SetActive(true);

            temp.SetMe(values[i]);
        }
    }
     public void SetAllToValue(float allValues)
    {
        for (int i = 0; i < nulBars.Count; i++)
        {

            //if (!coloursToShow.Contains((NoolColour)i))
            //{
            // continue;
            //}
            StarGraphSlice temp = nulBars[i];

            temp.gameObject.SetActive(true);

            temp.SetMe(allValues);
        }
    }

    public void SetToMerc(MercSheet mercSheet)
    {
        foreach (var nulElement in mercSheet.pillProfile.pills)
        {
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            if (!coloursToShow.Contains(nulElement.colour))
            {continue;}

            StarGraphSlice temp = nulBars[(int)nulElement.colour];

            temp.gameObject.SetActive(true);

            temp.SetMe(nulElement.potential);
        }
    }


}
