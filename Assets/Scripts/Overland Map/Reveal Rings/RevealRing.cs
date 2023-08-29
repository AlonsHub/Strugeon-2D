using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all types of Rings - site-reveal, enemy-identity-reveal, enemy-level-reveal //currently used as just site-reveal
public class RevealRing : MonoBehaviour
{
    public static RevealRing Instance;

    float siteRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity; 
    float idRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity; 
    float levelRevealIntensity => PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity; 
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
    public bool IsSiteInEnemyIDRing(SiteData siteData) => (idRevealIntensity >= siteData.logicalDistance);
    public bool IsSiteInEnemyLevelRing(SiteData siteData) => (levelRevealIntensity >= siteData.logicalDistance);
    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Debug.LogError($"more than one RevealRing! Destroyings {name} - under {transform.parent.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //siteRevealSlider.value = siteRevealIntensity;
        //idRevealSlider.value = idRevealIntensity;
        //levelRevealSlider.value = levelRevealIntensity;

        //siteRevealSlider.onValueChanged.AddListener(delegate { SetSiteReveal();});
        //idRevealSlider.onValueChanged.AddListener(delegate { SetIDReveal(); });
        //levelRevealSlider.onValueChanged.AddListener(delegate { SetLevelReveal(); });

    }

    //public void SetSiteReveal()
    //{
    //    //siteRevealIntensity = siteRevealSlider.value;
    //    siteRevealIntensity = PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity;
    //}
    //public void SetIDReveal()
    //{
    //    //idRevealIntensity = idRevealSlider.value;
    //    idRevealIntensity = PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity;
    //}
    //public void SetLevelReveal()
    //{
    //    //levelRevealIntensity = levelRevealSlider.value;
    //    levelRevealIntensity = PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity;

    //}
    //Gizmo parameters?
    //Gizmo? - ONLY RELEVANT if calculated and not manually set
}
