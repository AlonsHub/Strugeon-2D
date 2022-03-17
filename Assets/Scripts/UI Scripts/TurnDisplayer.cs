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

    //SA sorting section
    List<Transform> sa_Icons;
    [SerializeField]
    Transform SA_Parent;
    float saDistance = 50f;

    public void Init(Pawn pawn)
    {
        myPawn = pawn;
        pawnImage.sprite = pawn.PortraitSprite;
        iconBySAItem = new Dictionary<SA_Item, Image>();
       
        isScaled = false;
        hasSA = pawn.hasSAs;
        myPawn.myTurnPlate = this;
        sa_Icons = new List<Transform>();

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
                if (saItem.SA_Available())
                    SetSA_IconOn(saItem);
                else
                    SetSA_IconOff(saItem);
            }
            //saImage.gameObject.SetActive(myPawn.saItems.SA_Available());
        }
        //else
        //{
        //    saImage.gameObject.SetActive(myPawn._currentCooldown <= 0);
        //}
    }

    void SetSA_IconOn(SA_Item sai)
    {
        if(iconBySAItem.ContainsKey(sai))
        {
            //if one does exist, and is not active -> activate.
            //if active, it shouldn't happen

            
                iconBySAItem[sai].color = Color.white;

            return;
        }
        
        //if one does not exist, add one
        Image img = Instantiate(saIconPrefab, SA_Parent).GetComponent<Image>(); //maybe InChildren
        img.sprite = sai.SA_Sprite();
        img.color = Color.white;
        sa_Icons.Add(img.transform);
        SA_IconUpdate();
        //OnAddSAIcon(img.transform);
        iconBySAItem.Add(sai, img);
    }
    public void SetSA_IconOff(SA_Item sai)
    {
        if (!iconBySAItem.ContainsKey(sai))
        {
            Image img = Instantiate(saIconPrefab, SA_Parent).GetComponent<Image>(); //maybe InChildren
            img.sprite = sai.SA_Sprite();
            img.color = Color.gray;
            sa_Icons.Add(img.transform);
            SA_IconUpdate();
            //OnAddSAIcon(img.transform);
            iconBySAItem.Add(sai, img);
            return;
        }

        //Destroy(iconBySAItem[sai].gameObject);
        //iconBySAItem[sai].gameObject.SetActive(false);
        iconBySAItem[sai].color = Color.gray;
        SA_IconUpdate();

        //sa_Icons.Remove(iconBySAItem[sai].transform); //should remain
        //iconBySAItem.Remove(sai);
    }

    void SA_IconUpdate()
    {
        for (int i = 0; i < sa_Icons.Count; i++)
        {
            sa_Icons[i].localPosition = Vector3.zero + saDistance * Vector3.right * i;
        }
    }
}
