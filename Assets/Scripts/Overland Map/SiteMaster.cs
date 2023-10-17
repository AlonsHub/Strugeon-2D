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
    public NewSiteButton[] newSiteButtons; //for now, site buttons are the easiest way to get to site data

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


        //StartCoroutine(nameof(LateDiversify));
        StartCoroutine(nameof(NewLateDiversify));
    }

    private IEnumerator LateDiversify()
    {
        yield return new WaitForEndOfFrame();
        foreach (SiteButton sb in siteButtons)
        {
            if (!RevealRing.Instance.IsSiteInRing(sb.siteData))
            {
                sb.isReady = false;
                continue;
            }
            //Need to check if it is on cooldown?
            if (!sb.isCooldown && (!sb.levelSO.levelData.isSet || sb.levelSO.levelData.enemies == null))
            {
                
                Debug.Log("setting site");
                //sb.levelSO.levelData.SetLevelData((LairDifficulty)Random.Range(0, System.Enum.GetValues(typeof(LairDifficulty)).Length));
                sb.levelSO.levelData.SetLevelData(sb.GetRandomRelevantDifficulty());
            }
        }
        MakeSureSitesAreDiverese();
    }
    private IEnumerator NewLateDiversify()
    {
        yield return new WaitForEndOfFrame();
        foreach (NewSiteButton nsb in newSiteButtons)
        {
            if (!RevealRing.Instance.IsSiteInRing(nsb.siteData))
            {
                nsb.isReady = false;
                continue;
            }
            //Need to check if it is on cooldown?
            if (!nsb.isCooldown && (!nsb.levelSO.levelData.isSet || nsb.levelSO.levelData.enemies == null))
            {
                
                Debug.Log("setting site");
                //sb.levelSO.levelData.SetLevelData((LairDifficulty)Random.Range(0, System.Enum.GetValues(typeof(LairDifficulty)).Length));
                nsb.levelSO.levelData.SetLevelData(nsb.GetRandomRelevantDifficulty());
            }
        }
        //MakeSureSitesAreDiverese();
        MakeSureNewSitesAreDiverese();
    }

    //[ContextMenu("DiverseCheckOnSites")]
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
        SiteButton[] liveSites = siteButtons.Where(x => x.gameObject.activeInHierarchy && x.levelSO.levelData.isSet && x.relevantDifficulties.Length > 1).ToArray();

        if (liveSites.Length == 0)
            return;

        foreach (var item in liveSites)
        {
            //Debug.LogWarning($"{item.levelSO.name} isSet: {item.levelSO.levelData.isSet}");
            countsPerDifficulty[(int)item.levelSO.levelData.difficulty]++;
        }

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            if (countsPerDifficulty[i] >= liveSites.Length)
            {
                //need to reset one!
                //choose randomly, but set to a different difficulty - that is NOT i
                countsPerDifficulty[i]--; //because this one is being changed, and it will not be the same difficulty
                i++;
                if (i >= numberOfDifficulties)
                {
                    i = 0;
                }
                countsPerDifficulty[i]++;
                //should check if a random site is "good" reset
                Debug.Log("resetting site");
                //siteButtons[Random.Range(0, siteButtons.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);
                //liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);
                liveSites = liveSites.Where(x => x.relevantDifficulties.Contains((LairDifficulty)i)).ToArray();

                liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);

                break;
            }
        }

    }
    public void MakeSureNewSitesAreDiverese()
    {
        Debug.Log("DIVERSE!");

        int numberOfDifficulties = System.Enum.GetValues(typeof(LairDifficulty)).Length;
        int[] countsPerDifficulty = new int[numberOfDifficulties];

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            countsPerDifficulty[i] = 0;
        }
        //int liveSiteCount = 0;
        NewSiteButton[] liveSites = newSiteButtons.Where(x => x.gameObject.activeInHierarchy && x.levelSO.levelData.isSet && x.relevantDifficulties.Length > 1).ToArray();

        if (liveSites.Length == 0)
            return;

        foreach (var item in liveSites)
        {
            //Debug.LogWarning($"{item.levelSO.name} isSet: {item.levelSO.levelData.isSet}");
            countsPerDifficulty[(int)item.levelSO.levelData.difficulty]++;
        }

        for (int i = 0; i < numberOfDifficulties; i++)
        {
            if (countsPerDifficulty[i] >= liveSites.Length)
            {
                //need to reset one!
                //choose randomly, but set to a different difficulty - that is NOT i
                countsPerDifficulty[i]--; //because this one is being changed, and it will not be the same difficulty
                i++;
                if (i >= numberOfDifficulties)
                {
                    i = 0;
                }
                countsPerDifficulty[i]++;
                //should check if a random site is "good" reset
                Debug.Log("resetting site");
                //siteButtons[Random.Range(0, siteButtons.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);
                //liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);
                liveSites = liveSites.Where(x => x.relevantDifficulties.Contains((LairDifficulty)i)).ToArray();

                liveSites[Random.Range(0, liveSites.Length)].levelSO.levelData.SetLevelData((LairDifficulty)i);

                break;
            }
        }

    }


}
