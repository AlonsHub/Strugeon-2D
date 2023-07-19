using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
[System.Serializable]
public class RosterSlot : MonoBehaviour, IPointerClickHandler
{
    
    public Image img;
    public Image bg_img;
    [SerializeField]
    Sprite defaultPortraitSprite;
    [SerializeField]
    Sprite onBgSprite;
    [SerializeField]
    Sprite offBgSprite;
    
    
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    Image abilitySprite;
    [SerializeField]
    TMP_Text hpText;
    [SerializeField]
    TMP_Text damageText;
    [SerializeField]
    TMP_Text typeText;
    [SerializeField]
    ExpBarDisplayer expBarDisplayer;
    //public TMP_Text nameText;
    public bool isOccupied = false;
    public Pawn pawn;
    public MercSheet mercSheet;

    public bool isPartySlot;

    
    SquadBuilder2 squadBuilder => Tavern.Instance.squadBuilder;
    [SerializeField]
    GameObject mercDisplayer;


    public void SetMe(Pawn p)
    {
        gameObject.SetActive(true);
        nameText.text = p.mercName.ToString();
        typeText.text = p._mercSheet.mercClass.ToString();
        pawn = p;
        img.sprite = pawn.PortraitSprite;
        img.color = new Color(1, 1, 1, 1);
        isOccupied = true;

        expBarDisplayer.SetBar(p._mercSheet);

        hpText.text = $"HP: {p._mercSheet._maxHp}/{p._mercSheet._maxHp}";
        damageText.text = $"Damage: {p._mercSheet._minDamage}-{p._mercSheet._maxDamage}";
        abilitySprite.sprite = p.SASprite;
    }
    public void SetMe(MercSheet ms)
    {
        //gameObject.SetActive(true);
        mercSheet = ms;
        typeText.text = mercSheet.mercClass.ToString();

        expBarDisplayer.SetBar(mercSheet);

        img.color = new Color(1, 1, 1, 1);
        isOccupied = true;

        hpText.text = $"HP: {mercSheet._maxHp}/{mercSheet._maxHp}";
        damageText.text = $"Damage: {mercSheet._minDamage}-{mercSheet._maxDamage}";

        pawn = mercSheet.MyPawnPrefabRef<Pawn>();
        img.sprite = pawn.PortraitSprite;
        abilitySprite.sprite = pawn.SASprite;
        nameText.text = pawn.mercName.ToString();
       
    }


    public void SetMe()
    {
        gameObject.SetActive(true);

        nameText.text = "";
        typeText.text = "";

        expBarDisplayer.SetBar();


        img.sprite = defaultPortraitSprite;
        img.color = new Color(0,0,0,0);
        isOccupied = false;

        hpText.text = $"HP: XXX/XXX";
        damageText.text = $"Damage: XX-XX";

        abilitySprite.sprite = null;

    }

    public void RemoveMerc()
    {
        //on click, if a merc exists, move to relevant pool:
        if (isPartySlot)
        {
            //squadBuilder.tempSquad.RemoveMerc(pawn);
            squadBuilder.RemoveMercFromParty(pawn);
            PartyMaster.Instance.availableMercPrefabs.Add(pawn);

        }
        else
        {
            //squadBuilder.tempSquad.AddMerc(pawn);
            squadBuilder.AddMercToParty(pawn);
            PartyMaster.Instance.availableMercPrefabs.Remove(pawn);
        }
        isOccupied = false;
        FrameToggle(false);
        //SetMe();
        ClearSlot();
        squadBuilder.Refresh();
    }

    public void ClearSlot()
    {
        pawn = null;
        
        nameText.text = "";
        typeText.text = "";

        expBarDisplayer.SetBar();


        img.sprite = defaultPortraitSprite;
        img.color = new Color(0, 0, 0, 0);
        isOccupied = false;

        hpText.text = $"HP: XXX/XXX";
        damageText.text = $"Damage: XX-XX";

        abilitySprite.sprite = null;
        //nameText.text = "";
    }

   
    public void OnPointerClick(PointerEventData eventData) // SET BY OCDE
    {
        if (!isOccupied)
        {
            Debug.LogWarning("Slot has no Pawn");
            return;
        }

     
        if (eventData.button == PointerEventData.InputButton.Left) //No longer a distinction we really need to make
        {
                if (isPartySlot || squadBuilder.tempSquad.pawns.Count < squadBuilder.ToRoom.size)
                    RemoveMerc();
        }
    }

    bool isOneClicked = false;
    float doubleClickGraceTime = 1;
    IEnumerator DoubleClickWaiter()
    {
        isOneClicked = true;

        yield return new WaitForSeconds(doubleClickGraceTime);
        isOneClicked = false;

    }

    public void FrameToggle(bool turnOn)
    {
        if(turnOn)
        {
            bg_img.sprite = onBgSprite;
        }
        else
        {
            bg_img.sprite = offBgSprite;
        }
    }
}
