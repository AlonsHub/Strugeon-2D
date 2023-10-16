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
    [ContextMenu("DO IT - Only works if Main Camera is orthographic")]
    public void CreateRealWorldSites()
    {
        Camera mainCam = Camera.main;

        mainCam.orthographic = true;
        //cycle through sites
        foreach (var item in canvasSiteButtonsToConvert)
        {
            if (!item.gameObject.activeSelf)
                continue;

            GameObject go = PrefabUtility.InstantiatePrefab((UnityEngine.Object)realSitePrefab, newSitesParent) as GameObject;

            SpriteButton pb = go.GetComponentInChildren<SpriteButton>();
            
            //ONLY WORKS IF MAIN CAM IS ORTHO!
            go.transform.position = Camera.main.ScreenToWorldPoint(item.transform.position);
            
             pb.SetMe( item.thisButton, item.thisImage.sprite);
        }
        mainCam.orthographic = false;
        //spawn per site
        //position from ScreenToWorld 
        //set new parent and y value

        //init NewSite with OldSite data?
    }
}
