using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [ContextMenu("Objs to prefabs")]
    public void FixObjectsToBePrefabs()
    {
        foreach (var site in selectedSitesToFix)
        {
            SiteButton newSB = Instantiate(siteButtonPrefab, site.transform.parent).GetComponent<SiteButton>();
            newSB.transform.localPosition = site.transform.localPosition;
            newSB.levelSO = site.levelSO;
            newSB.pathCreator= site.pathCreator;
            newSB.siteData= site.siteData;

            SpriteState ss = site.thisButton.spriteState;
            newSB.thisButton.spriteState= ss;

            newSB.thisImage.sprite = site.thisImage.sprite;

            newSB.gameObject.name = site.gameObject.name;

            site.gameObject.SetActive(false);
        }

    }

   
}
