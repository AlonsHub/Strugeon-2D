using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPanel : MonoBehaviour
{
    [SerializeField]
    List<NoolColour> coloursToShow;

    [SerializeField, Tooltip("set in this order:Orange, Yellow, Green, Blue, Red, Purple, Black")]
    List<PillGraphSlice> noolPills;

    private void Start()
    {
        SetToPsion();
    }
    public void SetToPsion()
    {
        foreach (var nulElement in PlayerDataMaster.Instance.currentPlayerData.noolProfile.nools)
        {
            if(nulElement.currentValue <=0)
            {
                continue;
            }
            //NulBar nulBar = Instantiate(nulBarPrefab, nulBarParent).GetComponent<NulBar>();
            PillGraphSlice temp = noolPills[(int)nulElement.colour];
            if (!coloursToShow.Contains(nulElement.colour))
            {
                //temp.gameObject.SetActive(false); 
                continue;
            }

            temp.gameObject.SetActive(true);

            temp.SetMeFull(nulElement);
        }
    }

}
