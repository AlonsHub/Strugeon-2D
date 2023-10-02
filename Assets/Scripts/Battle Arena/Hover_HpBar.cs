using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;


public class Hover_HpBar : MonoBehaviour
{
    public static Hover_HpBar Instance;

    [SerializeField]
    public GameObject gfxToToggle;
    [SerializeField]
    Image mask;
    
    [SerializeField]
    TMPro.TMP_Text hpText;
    [SerializeField]
    TMPro.TMP_Text nameText;


    [SerializeField]
    Color enemyColour;
    [SerializeField]
    Color mercColour;

    Pawn pawn;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("This shouldnt happen - how do we 2 hover HP bars?");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetMe(Pawn p)
    {
        pawn = p;
        //spriteMaskTrans.localScale = new Vector3(((float)pawn.currentHP / (float)pawn.maxHP), 1, 1); //full hp is (2,1,1)
        mask.fillAmount = (float)(pawn.currentHP / (float)pawn.maxHP);

        mask.color = pawn.isEnemy ? enemyColour : mercColour;

        hpText.text = $"{pawn.currentHP} / {pawn.maxHP}";
        nameText.text = pawn.Name;
        gfxToToggle.SetActive(true);
    }

    public void SetOff()
    {
        gfxToToggle.SetActive(false);
    }


}
