using System.Collections;
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
        if(Instance != null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        siteButtons = FindObjectsOfType<SiteButton>(); //tbf AF

    }

    private void OnEnable()
    {
        //siteButtons = FindObjectsOfType<SiteButton>(); //tbf AF

        foreach (SiteButton sb in siteButtons)
        {
            if(!sb.levelSO.levelData.isSet || sb.levelSO.levelData.enemies == null)
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
        int numberOfDifficulties = System.Enum.GetValues(typeof(LairDifficulty)).Length;
        int[] countsPerDifficulty = new int[numberOfDifficulties];

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            countsPerDifficulty[i] = 0;
        }
        int liveSiteCount = 0;
        foreach (var item in siteButtons)
        {
            if (item.levelSO.levelData.isSet)
            {
                liveSiteCount++;
                countsPerDifficulty[(int)item.levelSO.levelData.difficulty]++;
            }
        }

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            if(countsPerDifficulty[i] >= liveSiteCount)
            {
                //need to reset one!
                //choose randomly, but set to a different difficulty - that is NOT i
                i++;
                if(i>= numberOfDifficulties)
                {
                    i = 0;
                }

                //should check if a random site is "good" reset
                Debug.Log("resetting site");
                siteButtons[Random.Range(0, siteButtons.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);

                break;
            }
        }
    }


}
