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
    List<DwellerDisplayer> dwellerDisplayers;

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
        if (dwellerDisplayers == null)
            dwellerDisplayers = new List<DwellerDisplayer>();

        if (Instances == null)
        {
            Instances = new List<SiteDisplayer>();
        }
        Instances.RemoveAll(x => x == null);

        Instances.Add(this);
    }
    public void SetMe(SiteButton sb) //TBF siteButton realtions are poblematic - they each have a sitedisplayer, why are they like thissss
    {
        siteButton = sb;
        levelData = sb.levelSO.levelData;

        for (int i = 0; i < levelData.enemies.Count; i++)
        {
            if (i >= dwellerDisplayers.Count)
            {
                
                DwellerDisplayer dweller = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>();

                dweller.SetMe(levelData.enemies[i], levelData.enemyLevels[i]); //THIS SHOULD CHECK FOR enemyLevelRing

                
                dwellerDisplayers.Add(dweller); //as image
            }
            else
            {
                
                dwellerDisplayers[i].SetMe(levelData.enemies[i], levelData.enemyLevels[i]);
            }
        }
        if (levelData.enemies.Count < dwellerDisplayers.Count) // BADDDD and easy to do otherwise
        {
            int delta = dwellerDisplayers.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                DwellerDisplayer dd = dwellerDisplayers[dwellerDisplayers.Count - 1].gameObject.GetComponentInParent<DwellerDisplayer>();
                dwellerDisplayers.RemoveAt(dwellerDisplayers.Count - 1);
                dd.KillMe();
            }
        } // BADDDD and easy to do otherwise

        difficultyImage.sprite = difficultySprties[(int)levelData.difficulty];
        goldDisplayer.GetComponent<TMPro.TMP_Text>().text = levelData.goldReward.ToString(); // come on, man...
    }
    //TBF siteButton realtions are poblematic - they each have a sitedisplayer, why are they like thissss
    public void SetMe(SiteButton sb, bool idReveal, bool levelReveal) //In this version - ID reveal ALWAYS precedes LevelReveal, so idReveal == false, assumes levelReveal is also false
    {
        siteButton = sb;
        levelData = sb.levelSO.levelData;

        for (int i = 0; i < levelData.enemies.Count; i++)
        {
            if (i >= dwellerDisplayers.Count)
            {
                
                DwellerDisplayer dweller = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>();

                if (idReveal)
                {
                    if (levelReveal)
                        dweller.SetMe(levelData.enemies[i], levelData.enemyLevels[i]); //THIS SHOULD CHECK FOR enemyLevelRing
                    else
                        dweller.SetMe(levelData.enemies[i]);
                }
                else
                {
                    dweller.SetMe();
                }

                
                dwellerDisplayers.Add(dweller); 
            }
            else
            {
                if (idReveal)
                {
                    if (levelReveal)
                        dwellerDisplayers[i].SetMe(levelData.enemies[i], levelData.enemyLevels[i]); //THIS SHOULD CHECK FOR enemyLevelRing
                    else
                        dwellerDisplayers[i].SetMe(levelData.enemies[i]);
                }
                else
                {
                    dwellerDisplayers[i].SetMe();
                }
            }
        }

        if (levelData.enemies.Count < dwellerDisplayers.Count) // BADDDD and easy to do otherwise
        {
            int delta = dwellerDisplayers.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                DwellerDisplayer dd = dwellerDisplayers[dwellerDisplayers.Count - 1].gameObject.GetComponentInParent<DwellerDisplayer>();
                dwellerDisplayers.RemoveAt(dwellerDisplayers.Count - 1);
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
            if (i >= dwellerDisplayers.Count)
            {
                //Image img = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>().portrait;
                DwellerDisplayer dweller = Instantiate(dwellerPortraitPrefab, dwellerPortraitGroupRoot).GetComponent<DwellerDisplayer>();
                //if (!img)
                //{
                //    Debug.LogError("not UI image found on dwellerPortraitPrefab");
                //}
                dweller.SetMe(levelData.enemies[i], levelData.enemyLevels[i]);
                //img.sprite = levelData.enemies[i].FullPortraitSprite; //consider keeping them somewhere
                dwellerDisplayers.Add(dweller); //as image
            }
            else
            {

                dwellerDisplayers[i].SetMe(levelData.enemies[i], levelData.enemyLevels[i]);
            }
        }
        if (levelData.enemies.Count < dwellerDisplayers.Count) // BADDDD and easy to do otherwise
        {
            int delta = dwellerDisplayers.Count - levelData.enemies.Count;
            for (int i = 1; i <= delta; i++)
            {
                DwellerDisplayer dd = dwellerDisplayers[dwellerDisplayers.Count - 1].gameObject.GetComponentInParent<DwellerDisplayer>();
                dwellerDisplayers.RemoveAt(dwellerDisplayers.Count - 1);
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
