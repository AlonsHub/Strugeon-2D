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
    [ContextMenu("DO IT")]
    public void CreateRealWorldSites()
    {
        //cycle through sites
        foreach (var item in canvasSiteButtonsToConvert)
        {
            if (!item.gameObject.activeInHierarchy)
                continue;

            GameObject go = Instantiate(realSitePrefab, newSitesParent);

            SpriteButton pb = go.GetComponentInChildren<SpriteButton>();

            //go.transform.position = Camera.main.ScreenToWorldPoint(item.GetComponent<RectTransform>().);
            go.transform.position = Camera.main.ScreenToWorldPoint(item.transform.position);
            //go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, go.transform.localPosition.z);
             pb.SetMe( item.thisButton, item.thisImage.sprite);
        }
        //spawn per site
        //position from ScreenToWorld 
        //set new parent and y value

        //init NewSite with OldSite data?
    }
}
