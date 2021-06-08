using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TurnDisplayer : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    //public TMP_Text nameDisplayer;
    //public TMP_Text initDisplayer;
    public Image pawnImage;
    public Image saImage;

    //public TurnTaker myPawn;
    public Pawn myPawn;

    public bool hasSA = false;

    public void Init(Pawn pawn)
    {
        myPawn = pawn;
        pawnImage.sprite = pawn.PortraitSprite;

        if(pawn.SASprite)
        {
            saImage.sprite = pawn.SASprite;
            hasSA = true;
        }
        else
        {
            saImage.gameObject.SetActive(false);
        }
        //nameDisplayer.text 

    }

    public void SAIconCheck()
    {
        saImage.gameObject.SetActive(myPawn._currentCooldown<=0);
    }

    
    

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (myPawn != null)
    //    {
    //        //Set Character Hover Display 
    //        CharacterHoverDisplayer.Instance.SetHoverToMerc((Pawn)myPawn, transform.position.x);
    //        //Display Character Hover Display
    //        CharacterHoverDisplayer.Instance.OnOffToggel(true);
    //    }

    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    CharacterHoverDisplayer.Instance.OnOffToggel(false);
    //    //Close Character Hover Display
    //}

}
