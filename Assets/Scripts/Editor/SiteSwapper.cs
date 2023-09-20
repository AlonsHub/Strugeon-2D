using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SiteSwapper : MonoBehaviour
{
    [SerializeField]
    GameObject realSitePrefab;
    [SerializeField]
    Transform newSitesParent;


    [SerializeField]
    SiteButton[] canvasSiteButtonsToConvert;

    public void CreateRealWorldSites()
    {
        //cycle through sites

        //spawn per site
        //position from ScreenToWorld 
        //set new parent and y value

        //init NewSite with OldSite data?
    }
}
