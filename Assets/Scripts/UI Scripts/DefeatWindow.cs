using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DefeatWindow : MonoBehaviour
{
    [SerializeField]
    GameObject mercPortraitPrefab;
    [SerializeField]
    Image siteImageDisplayer;

    [SerializeField]
    Transform mercPortraitGridParent;

    public void SetMe(LevelSO levelSO)
    {
        foreach (var pawn in PartyMaster.Instance.currentSquad.pawns)
        {
            GameObject go = Instantiate(mercPortraitPrefab, mercPortraitGridParent);
            //go.GetComponent<MercSummaryDisplayer>().SetMe(pawn.PortraitSprite, pawn.currentHP <= 0);
            //if (TurnMaster.Instance.theCowardly.Contains(pawn.mercName))
            if (RefMaster.Instance.GetTheCowardly.Contains(pawn.mercName))
                go.GetComponent<MercSummaryDisplayer>().SetMe(pawn.PortraitSprite, false, true);
            else
                go.GetComponent<MercSummaryDisplayer>().SetMe(pawn.PortraitSprite, true, false);
        }

        siteImageDisplayer.sprite = levelSO.levelData.siteIcon;
    }

    public void LoadTavern()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TavernScene");
    }
}
