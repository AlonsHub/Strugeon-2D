﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class ButtonFixer : MonoBehaviour
{
    //TEMP AF
    public Button[] buttons;
    
    public float aplhaHitMinimumThreshold;

    [SerializeField]
    GameObject siteButtonPrefab;
    [SerializeField]
    SiteButton[] selectedSitesToFix;
    
    void Awake()
    {
        //SO TEMPT OMG
        buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
            button.image.alphaHitTestMinimumThreshold = aplhaHitMinimumThreshold;
    }
#if UNITY_EDITOR
    [ContextMenu("Objs to prefabs")]
    public void FixObjectsToBePrefabs()
    {
        foreach (var site in selectedSitesToFix)
        {
            SiteButton newSB =   (PrefabUtility.InstantiatePrefab(siteButtonPrefab, site.transform.parent) as GameObject).GetComponent<SiteButton>();
            newSB.transform.localPosition = site.transform.localPosition;
            
            newSB.levelSO = site.levelSO;
            newSB.pathCreator= site.pathCreator;
            newSB.siteData= site.siteData;

            SpriteState ss = site.thisButton.spriteState;
            newSB.thisButton.spriteState= ss;

            newSB.thisImage.sprite = site.thisImage.sprite;

            newSB.gameObject.name = site.gameObject.name;

            newSB.thisButton.onClick.AddListener(newSB.OnClick);

            site.gameObject.SetActive(false);
        }

    }
#endif

}
