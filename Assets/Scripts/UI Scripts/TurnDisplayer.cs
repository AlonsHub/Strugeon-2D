using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TurnDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameDisplayer;
    public TMP_Text initDisplayer;

    public Image pawnImage;

    public TurnTaker myPawn;

    public void SetImage(Sprite newSprite)
    {
        pawnImage.sprite = newSprite;
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myPawn != null)
        {
            //Set Character Hover Display 
            CharacterHoverDisplayer.Instance.SetHoverToMerc((Pawn)myPawn, transform.position.x);
            //Display Character Hover Display
            CharacterHoverDisplayer.Instance.OnOffToggel(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CharacterHoverDisplayer.Instance.OnOffToggel(false);
        //Close Character Hover Display
    }

}
