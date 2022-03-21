using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteDisplayer : MonoBehaviour
{
    public static List<SiteDisplayer> Instances;

    [SerializeField]
    Image difficultyImage;
    [SerializeField]
    GameObject dwellerPortraitPrefab;
    [SerializeField]
    Transform dwellerPortraitGroupRoot;
    [SerializeField]
    List<Image> dwellerPortraitImgs;

    [SerializeField]
    Transform goldDisplayer;

    LevelData levelData;

    [SerializeField]
    Sprite[] difficultySprties;

    [SerializeField] //readolny
     bool isSiteSet;

    [SerializeField]
    SiteButton siteButton;
    //[SerializeField]
    //SimpleSiteButton simpleSiteButton;

    private void Awake()
    {
        if(Instances == null)
        {
            Instances = new List<SiteDisplayer>();
        }
        Instances.RemoveAll(x => x == null);

        Instances.Add(this);
    }
    public void SetMe(SiteButton sb)
    {
        siteButton = sb;
        levelData = sb.levelSO.levelData;

        for (int i = 0; i < levelData.enemies.Count; i++)
        {
            if (i >= dwellerPortraitImgs.Count)
            {
                Image img = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>().portrait;
                if (!img)
                {
                    Debug.LogError("not UI image found on dwellerPortraitPrefab");
                }
                img.sprite = levelData.enemies[i].FullPortraitSprite; //consider keeping them somewhere
                dwellerPortraitImgs.Add(img);
            }
            else
            {
                dwellerPortraitImgs[i].sprite = levelData.enemies[i].FullPortraitSprite;
            }
        }
        if (levelData.enemies.Count < dwellerPortraitImgs.Count) // BADDDD and easy to do otherwise
        {
            int delta = dwellerPortraitImgs.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                DwellerDisplayer dd = dwellerPortraitImgs[dwellerPortraitImgs.Count - 1].gameObject.GetComponentInParent<DwellerDisplayer>();
                dwellerPortraitImgs.RemoveAt(dwellerPortraitImgs.Count - 1);
                dd.KillMe();
            }
        } // BADDDD and easy to do otherwise

        difficultyImage.sprite = difficultySprties[(int)levelData.difficulty];
        goldDisplayer.GetComponent<TMPro.TMP_Text>().text = levelData.goldReward.ToString(); // come on, man...
    }

    public void SetMe(SimpleSiteButton ssb)
    {
        //siteButton = sb;
        levelData = ssb.LevelSO.levelData;

        for (int i = 0; i < levelData.enemies.Count; i++)
        {
            if (i >= dwellerPortraitImgs.Count)
            {
                Image img = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>().portrait;
                if (!img)
                {
                    Debug.LogError("not UI image found on dwellerPortraitPrefab");
                }
                img.sprite = levelData.enemies[i].FullPortraitSprite; //consider keeping them somewhere
                dwellerPortraitImgs.Add(img);
            }
            else
            {
                dwellerPortraitImgs[i].sprite = levelData.enemies[i].FullPortraitSprite;
            }
        }
        if (levelData.enemies.Count < dwellerPortraitImgs.Count) // BADDDD and easy to do otherwise
        {
            int delta = dwellerPortraitImgs.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                DwellerDisplayer dd = dwellerPortraitImgs[dwellerPortraitImgs.Count - 1].gameObject.GetComponentInParent<DwellerDisplayer>();
                dwellerPortraitImgs.RemoveAt(dwellerPortraitImgs.Count - 1);
                dd.KillMe();
            }
        } // BADDDD and easy to do otherwise

        difficultyImage.sprite = difficultySprties[(int)levelData.difficulty];
        goldDisplayer.GetComponent<TMPro.TMP_Text>().text = levelData.goldReward.ToString(); // come on, man...
    }

    //private void OnDisable()
    //{
    //    if (siteButton)
    //        siteButton.SetHoverColor(false);
    //}


    public static void SetActiveToAllInstances(bool isActive) //may be depricated soon [24/02/22] TBF
    {
        foreach (var item in Instances)
        {
            item.gameObject.SetActive(isActive);
        }
    }
}
