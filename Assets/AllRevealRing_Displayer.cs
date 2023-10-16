using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gold and Item reward are united at the moment
public enum RevealRingType {Site, Difficulty, EnemyAmount, EnemyID, EnemyLevel, Reward}; //TBF moved somewhere more prominent and also the order?
public class AllRevealRing_Displayer : MonoBehaviour
{

    

    [SerializeField]
    GameObject[] imgs;
    [SerializeField]
    GameObject toToggle;



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
        //imgs[(int)RevealRingType.Site].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity;
        //imgs[(int)RevealRingType.Difficulty].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.difficultyRevealIntensity;
        //imgs[(int)RevealRingType.EnemyAmount].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.enemyAmountRevealIntensity;
        //imgs[(int)RevealRingType.EnemyID].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.idRevealIntensity;
        //imgs[(int)RevealRingType.EnemyLevel].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.levelRevealIntensity;
        //imgs[(int)RevealRingType.Reward].transform.localScale = Vector3.one * PlayerDataMaster.Instance.currentPlayerData.rewardRevealIntensity;
        //Gizmos.DrawSphere(imgs[0].transform.position, PlayerDataMaster.Instance.currentPlayerData.siteRevealIntensity);
    }
   
    public void TurnOn()
    {
        SetRight();
        toToggle.SetActive(true);
        //foreach (var item in imgs)
        //{
        //    item.gameObject.SetActive(true);
        //}
    }
    public void TurnOff()
    {
        toToggle.SetActive(false);

        //foreach (var item in imgs)
        //{
        //    item.gameObject.SetActive(false);
        //}
    }

}
