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
    public Image[] saImages;

    //public TurnTaker myPawn;
    public Pawn myPawn;

    public bool hasSA = false;

    public GameObject saIconPrefab;

    public void Init(Pawn pawn)
    {
        myPawn = pawn;
        pawnImage.sprite = pawn.PortraitSprite;

        //if(pawn.SASprite)
        //{
        //    saImage.sprite = pawn.SASprite;
        //    hasSA = true;
        //}
        //else
        //{
        //    saImage.gameObject.SetActive(false);
        //}
        hasSA = pawn.hasSAs;
        if (hasSA)
        {
            saImages = new Image[pawn.saItems.Length];
            foreach (var saItem in pawn.saItems)
            {
                Image img = Instantiate(saIconPrefab, gameObject.transform).GetComponent<Image>(); //maybe InChildren
                img.sprite = saItem.SA_Sprite();
            }

        }
        //nameDisplayer.text 

    }

    public void SAIconCheck()
    {
        if (myPawn.saItems != null)
        {
            foreach (var saItem in myPawn.saItems)
            {
                saItem.SA_Available();
            }
            saImage.gameObject.SetActive(myPawn.saItems.SA_Available());
        }
        else
        {
            saImage.gameObject.SetActive(myPawn._currentCooldown <= 0);
        }
    }

    void AddSAIcon()
    {
        //consider addind to a dict also
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
