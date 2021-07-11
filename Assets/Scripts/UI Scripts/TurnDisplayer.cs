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

    Dictionary<SA_Item, Image> iconBySAItem;

    public void Init(Pawn pawn)
    {
        myPawn = pawn;
        pawnImage.sprite = pawn.PortraitSprite;
        iconBySAItem = new Dictionary<SA_Item, Image>();
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
        //if (hasSA)
        //{
        //    saImages = new Image[pawn.saItems.Length];
        //    foreach (var saItem in pawn.saItems)
        //    {
        //        AddSAIcon(saItem);
        //    }

        //}
        //nameDisplayer.text 

    }

    public void SAIconCheck()
    {
        if (myPawn.saItems != null)
        {
            foreach (var saItem in myPawn.saItems)
            {
                if(saItem.SA_Available())
                {
                    AddSAIcon(saItem);
                }
                else
                {
                    RemoveSAIcon(saItem);
                }

            }
            //saImage.gameObject.SetActive(myPawn.saItems.SA_Available());
        }
        //else
        //{
        //    saImage.gameObject.SetActive(myPawn._currentCooldown <= 0);
        //}
    }

    void AddSAIcon(SA_Item sai)
    {
        if(iconBySAItem.ContainsKey(sai))
        {
            Debug.LogError("TurnDisplayer Already containts this SA_Item Icon: " + sai.SA_Name());
            return;
        }
        
        Image img = Instantiate(saIconPrefab, gameObject.transform).GetComponent<Image>(); //maybe InChildren
        img.sprite = sai.SA_Sprite();
        iconBySAItem.Add(sai, img);
    }
    public void RemoveSAIcon(SA_Item sai)
    {
        if (!iconBySAItem.ContainsKey(sai))
        {
            Debug.Log("no SA_Icon of type: " + sai.SA_Name() + " to remove");
            return;
        }


        Destroy(iconBySAItem[sai].gameObject);
        iconBySAItem.Remove(sai);
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
