using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all types of Rings - site-reveal, enemy-identity-reveal, enemy-level-reveal //currently used as just site-reveal
public class RevealRing : MonoBehaviour
{
    public static RevealRing Instance;

    float siteRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity; 
    float enemyAmountIntensity => PlayerDataMaster.Instance.currentPlayerData.enemyAmountRevealIntensity; 
    float idRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity; 
    float levelRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity; 
    float rewardRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.rewardRevealIntensity; 
    //float siteRevealIntensity { get => PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity; set => PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity = value; }

    //float idRevealIntensity { get => PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity; set => PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity = value; }

    //float levelRevealIntensity { get => PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity; set => PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity = value; }

    //[SerializeField]
    //UnityEngine.UI.Slider siteRevealSlider;
    //[SerializeField]
    //UnityEngine.UI.Slider idRevealSlider;
    //[SerializeField]
    //UnityEngine.UI.Slider levelRevealSlider;

    public bool IsSiteInRing(SiteData siteData) => (siteRevealIntensity >= siteData.logicalDistance); // this simplified check assumes all sites would be MANUALLY (in overlandmap scene) (pre)set each site with logicalDistances
    //As it is right now 09/05/22, the code does NOT need to "calcualte" any site's logicalDistance - as they are manually set
    //we should, however, find a better approach (one that allows us to simply place a site on the map, and logicalDistance would be set using a DistanceCalc() method)
    public bool IsSiteInEnemyAmountRing(SiteData siteData) => (enemyAmountIntensity >= siteData.logicalDistance);
    public bool IsSiteInEnemyIDRing(SiteData siteData) => (idRevealIntensity >= siteData.logicalDistance);
    public bool IsSiteInEnemyLevelRing(SiteData siteData) => (levelRevealIntensity >= siteData.logicalDistance);
    public bool IsSiteInRewardRing(SiteData siteData) => (rewardRevealIntensity >= siteData.logicalDistance);
    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Debug.LogError($"more than one RevealRing! Destroyings {name} - under {transform.parent.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int ExposureLevel(SiteData siteData)
    {
        int toReturn= -1; // not at all

        if (!IsSiteInRing(siteData))
        {
            return -1;
        }
        else if(!IsSiteInEnemyAmountRing(siteData))
        {
            return (int)RevealRingType.Site;
        }
        else if(!IsSiteInEnemyIDRing(siteData))
        {
            return (int)RevealRingType.EnemyAmount;
        }
        else if(!IsSiteInEnemyLevelRing(siteData))
        {
            return (int)RevealRingType.EnemyID;
        }
        else if(!IsSiteInRewardRing(siteData))
        {
            return (int)RevealRingType.EnemyLevel;
        }
        else
        {
            return (int)RevealRingType.Reward;
        }
    }
}
