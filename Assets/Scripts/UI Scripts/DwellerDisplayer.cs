using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DwellerDisplayer : MonoBehaviour
{
    public Image portrait; //public since the SiteDisplayer keeps dwellerdisplayers as images... TBF
    
    [SerializeField]
    TMPro.TMP_Text level;
    [SerializeField]
    Sprite hiddenSprite;
    [SerializeField]
    Image questionMark;
    [SerializeField]
    TMPro.TMP_Text nameText;
    [SerializeField]
    GameObject levelBgBox;

    public void SetMe(Pawn p, int levelText)
    {
        questionMark.gameObject.SetActive(false);
        portrait.gameObject.SetActive(true);
        levelBgBox.SetActive(true);
        portrait.sprite = p.FullPortraitSprite;
        level.text = levelText.ToString();
        nameText.text = p.Name;
    }    
    public void SetMe(Pawn p)
    {
        questionMark.gameObject.SetActive(false);
        portrait.gameObject.SetActive(true);
        portrait.sprite = p.FullPortraitSprite;
        level.text = "";
        levelBgBox.SetActive(false);
        nameText.text = p.Name;
    }
    public void SetMe()
    {
        questionMark.gameObject.SetActive(true);
        portrait.gameObject.SetActive(false);
        levelBgBox.SetActive(false);
        level.text = "";
        nameText.text = "";
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
