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
    Image abilitySprite;
    [SerializeField]
    TMP_Text hpText;
    [SerializeField]
    TMP_Text damageText;
    //[SerializeField]
    //TMP_Text Text;

    //public TMP_Text nameText;
    public bool isOccupied = false;
    public Pawn pawn;

    public bool isPartySlot;

    
    SquadBuilder2 squadBuilder => Tavern.Instance.squadBuilder;
    [SerializeField]
    GameObject mercDisplayer;


    public void SetMe(Pawn p)
    {
        gameObject.SetActive(true);
        pawn = p;
        img.sprite = pawn.PortraitSprite;
        img.color = new Color(1, 1, 1, 1);
        isOccupied = true;

        hpText.text = $"HP: {p._mercSheet._maxHp}/{p._mercSheet._maxHp}";
        damageText.text = $"Damage: {p._mercSheet._minDamage}-{p._mercSheet._maxDamage}";
        abilitySprite.sprite = p.SASprite;
    }
    public void SetMe(MercSheet ms)
    {
        gameObject.SetActive(true);

        pawn = ms.MyPawnPrefabRef<Pawn>();
        img.sprite = pawn.PortraitSprite;
        img.color = new Color(1, 1, 1, 1);
        isOccupied = true;

        hpText.text = $"HP: {ms._maxHp}/{ms._maxHp}";
        damageText.text = $"Damage: {ms._minDamage}-{ms._maxDamage}";
        abilitySprite.sprite = pawn.SASprite;
    }


    public void SetMe()
    {
        gameObject.SetActive(true);

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
        squadBuilder.Refresh();
    }

    public void ClearSlot()
    {
        pawn = null;
        img.sprite = defaultPortraitSprite;
        img.color = new Color(0, 0, 0, 0);
        bg_img.sprite = offBgSprite;
        isOccupied = false;
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
