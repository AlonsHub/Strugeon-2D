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
    public void SetMe(Pawn p, int levelText)
    {
        portrait.sprite = p.FullPortraitSprite;
        level.text = levelText.ToString();
    }    
    public void SetMe(Pawn p)
    {
        portrait.sprite = p.FullPortraitSprite;
        level.text = "";
        //level.text = levelText.ToString();
    }
    public void SetMe()
    {
        portrait.sprite = hiddenSprite;
        level.text = "";
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }
}
