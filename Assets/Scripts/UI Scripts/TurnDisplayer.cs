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

    bool isScaled;

    public void Init(Pawn pawn)
    {
        myPawn = pawn;
        pawnImage.sprite = pawn.PortraitSprite;
        iconBySAItem = new Dictionary<SA_Item, Image>();
       
        isScaled = false;
        hasSA = pawn.hasSAs;
       
    }
    public void ToggleScale(bool scaleUp)
    {
        if (scaleUp == isScaled)
            return;

        transform.localScale = (scaleUp) ? transform.localScale * 1.5f : transform.localScale / 1.5f;
        isScaled = scaleUp;
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
            //Debug.LogError("TurnDisplayer Already containts this SA_Item Icon: " + sai.SA_Name());
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
            Debug.LogWarning("no SA_Icon of type: " + sai.SA_Name() + " to remove");
            return;
        }
        Destroy(iconBySAItem[sai].gameObject);
        iconBySAItem.Remove(sai);
    }
}
