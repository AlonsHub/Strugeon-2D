﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable] 
public class SiteMaster : MonoBehaviour
{
    public static SiteMaster Instance;
    //this can be easily saved and loaded as all site's data

    ///it will hold: 
    ///the available sites, controlling "map-progressing" (waiting from game-designed - WFGD)
    ///all expeditions and their data+state?
    ///site dwellers, difficulty, rewards - can also be saved? so players won't be able to restart the game to get new sites WFGD
    ///

    //temp - will just know of SiteButtons and ongoing expedition, should fix soon though TBF
    public SiteButton[] siteButtons; //for now, site buttons are the easiest way to get to site data

    public UnityEngine.UI.Button tavernButton;

    private void Awake()
    {
        //if(Instance != null && Instance !=this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Instance = this;
        //siteButtons = FindObjectsOfType<SiteButton>(); //tbf AF

    }
    //private void OnDisable()
    //{
    //    Instance = null;
    //}

    private void OnEnable()
    {
        //siteButtons = FindObjectsOfType<SiteButton>(); //tbf AF

        StartCoroutine(nameof(LateDiversify));
    }

    private IEnumerator LateDiversify()
    {
        yield return new WaitForEndOfFrame();
        foreach (SiteButton sb in siteButtons)
        {
            //Need to check if it is on cooldown?
            if (!sb.isCooldown && (!sb.levelSO.levelData.isSet || sb.levelSO.levelData.enemies == null))
            {
                Debug.Log("setting site");
                sb.levelSO.levelData.SetLevelData((LairDifficulty)Random.Range(0, System.Enum.GetValues(typeof(LairDifficulty)).Length));
            }
        }
        MakeSureSitesAreDiverese();
    }

    [ContextMenu("DiverseCheckOnSites")]
    public void MakeSureSitesAreDiverese()
    {
        Debug.Log("DIVERSE!");

        int numberOfDifficulties = System.Enum.GetValues(typeof(LairDifficulty)).Length;
        int[] countsPerDifficulty = new int[numberOfDifficulties];

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            countsPerDifficulty[i] = 0;
        }
        //int liveSiteCount = 0;
        SiteButton[] liveSites = siteButtons.Where(x => x.levelSO.levelData.isSet).ToArray();

        foreach (var item in liveSites)
        {
            Debug.LogWarning($"{item.levelSO.name} isSet: {item.levelSO.levelData.isSet}");
            countsPerDifficulty[(int)item.levelSO.levelData.difficulty]++;
        }

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            if(countsPerDifficulty[i] >= liveSites.Length)
            {
                //need to reset one!
                //choose randomly, but set to a different difficulty - that is NOT i
                countsPerDifficulty[i]--; //because this one is being changed, and it will not be the same difficulty
                i++;
                if(i>= numberOfDifficulties)
                {
                    i = 0;
                }
                countsPerDifficulty[i]++;
                //should check if a random site is "good" reset
                Debug.Log("resetting site");
                //siteButtons[Random.Range(0, siteButtons.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);
                liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);

                break;
            }
        }

        if(countsPerDifficulty[(int)LairDifficulty.Easy] <= 0)
        {
            liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData(LairDifficulty.Easy);
        }
    }


}
