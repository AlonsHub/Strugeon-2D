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
    public void SetMe(Pawn p, int levelText)
    {
        questionMark.gameObject.SetActive(false);
        portrait.gameObject.SetActive(true);
        portrait.sprite = p.FullPortraitSprite;
        level.text = levelText.ToString();
    }    
    public void SetMe(Pawn p)
    {
        questionMark.gameObject.SetActive(false);
        portrait.gameObject.SetActive(true);
        portrait.sprite = p.FullPortraitSprite;
        level.text = "";
    }
    public void SetMe()
    {
        questionMark.gameObject.SetActive(true);
        portrait.gameObject.SetActive(false);
        level.text = "";
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
