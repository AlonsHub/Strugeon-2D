using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gold and Item reward are united at the moment
public enum RevealRingType {Site, EnemyAmount, EnemyID, EnemyLevel, Reward}; //TBF moved somewhere more prominent and also the order?
public class AllRevealRing_Displayer : MonoBehaviour
{

    

    [SerializeField]
    GameObject[] imgs;

    private void OnEnable()
    {
        GameStats.OnRevealRadiusChanged += SetRight;
        SetRight();
    }
    private void OnDisable()
    {
        GameStats.OnRevealRadiusChanged -= SetRight;
    }
    void SetRight()
    {
        imgs[(int)RevealRingType.Site].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity;
        imgs[(int)RevealRingType.EnemyAmount].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.enemyAmountRevealIntensity;
        imgs[(int)RevealRingType.EnemyID].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity;
        imgs[(int)RevealRingType.EnemyLevel].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity;
        imgs[(int)RevealRingType.Reward].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.rewardRevealIntensity;
    }

    public void TurnOn()
    {
        SetRight();
        foreach (var item in imgs)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void TurnOff()
    {
        foreach (var item in imgs)
        {
            item.gameObject.SetActive(false);
        }
    }

}
