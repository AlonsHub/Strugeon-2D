using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Currently the only prefab relevant for this is the SimpleGearSlot - GearSlot1 is deprecated
public class MercGearDisplayer : BasicDisplayer
{
    public static MercGearDisplayer Instance;

    MercSheet mercSheet;


    [SerializeField]
    GearSlotDispayer[] gearSlots = new GearSlotDispayer[3]; // by EquipSlotType

    [SerializeField]
    ExpBarDisplayer expBarDisplayer;

    public MercSheet GetMercSheet { get => mercSheet; }

    public System.Action OnMercChange;

    int current = 0;

    private void Awake()
    {
        //TBF TBD THIS IS LAZY
        Instance = this;
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //SetMeFully(PlayerDataMaster.Instance.GetMercSheetByName());
        SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(current)); // should actually look for the first AVAILABLE merc! TBF
    }

    public void SetMeFully(MercSheet ms)
    {
        mercSheet = ms;
        expBarDisplayer.SetBar(ms);
        Pawn p = ms.MyPawnPrefabRef<Pawn>();

        int maxHpBenefit = 0;
        int damageBenefit = 0;

        foreach (var benefit in ms.gear.GetAllBenefits())
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

        base.SetMe(new List<string> {ms.characterName.ToString(),$"{ms._maxHp}\n<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{maxHpBenefit}</color>",$"{ms._minDamage} - {ms._maxDamage}\n<color=#{ColorUtility.ToHtmlStringRGBA(Color.green)}>+{damageBenefit}</color>", p.SA_Title, p.SA_Description}, new List<Sprite> {p.FullPortraitSprite, p.SASprite});
        DisplayGear();
        OnMercChange?.Invoke();
        //foreach of that Mercs items - display them in their relevant slots
    }

    public void DisplayGear()
    {
        for (int i = 0; i < gearSlots.Length; i++)
        {
            IEquipable eq;
            if ((eq = mercSheet.gear.GetItemBySlot((EquipSlotType)i)) != null)
            {
                gearSlots[i].SetMeFull(eq as MagicItem, mercSheet, null); //should not work
            }
            else
            {
                gearSlots[i].SetMeEmpty();
            }
        }
    }

    public void TryEquipItem(MagicItem item)
    {
        //remove from inventory
        Inventory.Instance.RemoveMagicItem(item);
        if (mercSheet == null)
            return;
        IEquipable removedItem;
        if((removedItem = mercSheet.gear.TryEquipItemToSlot(item)) !=null)
        {
            //add to inventory
            Inventory.Instance.AddMagicItem(removedItem as MagicItem);
        }

        DisplayGear(); //refreshes
        SetMeFully(mercSheet);
    }

    public void ArrowNavigation(int i)
    {
        current += i;
        if(current >= PlayerDataMaster.Instance.currentPlayerData.mercSheets.Count)
        {
            current = 0;
        }
        if(current < 0)
        {
            current = PlayerDataMaster.Instance.currentPlayerData.mercSheets.Count - 1;
        }

        SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(current)); // should actually look for the first AVAILABLE merc! TBF
    }

}
