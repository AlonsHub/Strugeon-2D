﻿using System.Collections;
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
        

        maxObject.gameObject.SetActive(true);
        minObject.gameObject.SetActive(false);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (mercSheet == null)
            return;

        
        minObject.gameObject.SetActive(true);
        maxObject.gameObject.SetActive(false);
    }

    public void SetMe(MercSheet ms)
    {
        gameObject.SetActive(true);
        mercSheet = ms;
        minObject.gameObject.SetActive(true);
        minObject.SetMeFull(ms);
        maxObject.SetMe(ms);
    }

    public void SetToEmpty()
    {
        gameObject.SetActive(true);

        mercSheet = null;
        minObject.gameObject.SetActive(false);
        maxObject.gameObject.SetActive(false);
    }




}
