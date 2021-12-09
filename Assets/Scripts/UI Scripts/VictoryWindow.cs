using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text goldDisplayer;
    [SerializeField]
    GameObject enemyPortraitPrefab;
    [SerializeField]
    GameObject mercPortraitPrefab;
    [SerializeField]
    Image siteImageDisplayer;

    [SerializeField]
    Transform enemyPortraitGridParent;
    [SerializeField]
    Transform mercPortraitGridParent;

    public void SetMe(LevelSO levelSO)
    {
        goldDisplayer.text = levelSO.levelData.goldReward.ToString();

        //Debug.LogError(levelSO.levelData.enemies.Count);

        foreach (var pawn in levelSO.levelData.enemies)
        {
            GameObject go = Instantiate(enemyPortraitPrefab, enemyPortraitGridParent);
            //go.GetComponentInChildren<Image>().sprite = pawn.PortraitSprite;
            go.GetComponent<EnemySummary>().SetMe( pawn.PortraitSprite);
        }

        foreach (var pawn in RefMaster.Instance.mercs)
        {
            GameObject go = Instantiate(mercPortraitPrefab, mercPortraitGridParent);
            go.GetComponent<MercSummaryDisplayer>().SetMe(pawn.PortraitSprite, pawn.currentHP<=0, false);
        }
        foreach (var pawnName in TurnMaster.Instance.theDead)
        {
            PlayerDataMaster.Instance.currentPlayerData.deadMercs++;


            GameObject go = Instantiate(mercPortraitPrefab, mercPortraitGridParent);
            Pawn p = MercPrefabs.Instance.EnumToPawnPrefab(pawnName);
            go.GetComponent<MercSummaryDisplayer>().SetMe(p.PortraitSprite, true, false);
        }
        foreach (var pawnName in TurnMaster.Instance.theCowardly)
        {
            PlayerDataMaster.Instance.currentPlayerData.cowardMercs++;

            GameObject go = Instantiate(mercPortraitPrefab, mercPortraitGridParent);
            Pawn p = MercPrefabs.Instance.EnumToPawnPrefab(pawnName); //need enum to portrait in stead (dict)
            go.GetComponent<MercSummaryDisplayer>().SetMe(p.PortraitSprite, false, true);
        }

        siteImageDisplayer.sprite = levelSO.levelData.siteIcon;
    }

    public void LoadTavern()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TavernScene");
    }
}
