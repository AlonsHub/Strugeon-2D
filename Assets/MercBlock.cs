using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MercBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    MercSheet mercSheet;

    [SerializeField]
    MiniMercBlock minObject;
    [SerializeField]
    RosterSlot maxObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mercSheet == null)
            return;
        //minObject.gameObject.SetActive(false);
        maxObject.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (mercSheet == null)
            return;
        //minObject.gameObject.SetActive(true);
        maxObject.gameObject.SetActive(false);
    }

    public void SetMe(MercSheet ms)
    {
        mercSheet = ms;
        minObject.gameObject.SetActive(true);
        minObject.SetMeFull(ms);
        maxObject.SetMe(ms);
    }

    public void SetToEmpty()
    {
        mercSheet = null;
        minObject.gameObject.SetActive(false);
        maxObject.gameObject.SetActive(false);
    }




}
