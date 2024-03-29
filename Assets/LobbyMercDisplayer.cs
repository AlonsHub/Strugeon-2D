﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMercDisplayer : BasicDisplayer
{
    [SerializeField]
    ExpBarDisplayer expBarDisplayer;
    MercPoolDisplayer mercPoolDisplayer;
    MercSheet sheet;

    [SerializeField, Tooltip("Not required")]
    MercGearDisplayer mgd;


    [SerializeField]
    GearSlotDispayer[] gearSlots = new GearSlotDispayer[3]; // by EquipSlotType

    public bool SetMeFull(MercSheet mercSheet, MercPoolDisplayer poolDisplayer)
    {
        mercPoolDisplayer = poolDisplayer;
        sheet = mercSheet;

        Pawn p = mercSheet.MyPawnPrefabRef<Pawn>();

        int maxHpBenefit = 0;
        int damageBenefit = 0;

        foreach (var benefit in mercSheet.gear.GetAllBenefits())
        {
            switch ((benefit as StatBenefit).statToBenefit)
            {
                case StatToBenefit.MaxHP:
                    maxHpBenefit += benefit.Value();
                    break;
                case StatToBenefit.FlatDamage:
                    damageBenefit += benefit.Value();
                    break;
                default:
                    break;
            }
        }
        string assingmentText = "";
        switch (mercSheet.currentAssignment)
        {
            case MercAssignment.Null:
                break;
            case MercAssignment.AwaySquad:
                assingmentText = $"On route";


                break;
            case MercAssignment.Room:
                assingmentText = $"in room {mercSheet.roomOrSquadNumber+1}";
                break;
            case MercAssignment.Available:
                assingmentText = "Available";
                break;
            //case MercAssignment.Hireable:
            //    break;
            //case MercAssignment.NotAvailable:
            //    break;
            default:
                assingmentText = mercSheet.currentAssignment.ToString();
                break;
        }
        List<string> textsPerTextBox = new List<string> 
        { mercSheet.characterName.ToString(),
            //mercSheet.currentAssignment.ToString(), 
            assingmentText,
            $"{mercSheet._maxHp}(<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{maxHpBenefit}</color>)", 
            $"{mercSheet._minDamage} - {mercSheet._maxDamage}(<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{damageBenefit}</color>)" };

        List<Sprite> spritesPerImage = new List<Sprite> {p.PortraitSprite, p.SASprite};


        expBarDisplayer.SetBar(mercSheet);

        DisplayGear();

        return base.SetMe(textsPerTextBox, spritesPerImage);
    }


    public void DisplayGear()
    {
        for (int i = 0; i < gearSlots.Length; i++)
        {
            IEquipable eq;
            if ((eq = sheet.gear.GetItemBySlot((EquipSlotType)i)) != null)
            {
                gearSlots[i].SetMeFull(eq as MagicItem, sheet,this);
            }
            else
            {
                gearSlots[i].SetMeEmpty();
            }
        }
    }

    public void OpenMercIndividualWindow() //called by arrowbuttons in inspector
    {
        MercGearDisplayer.Instance.SetMeFully(sheet);
        MercGearDisplayer.Instance.gameObject.SetActive(true);
    }
    //Need this for MercGearDisplayer - currently the Merc View Winodow!
    //public void OnMyClick()
    //{
    //    //display merc to the MercGearDisplayer
    //    if(sheet.currentAssignment != MercAssignment.AwaySquad)
    //    mercPoolDisplayer.ShowMercOnGearDisplayer(sheet);
    //}
}
