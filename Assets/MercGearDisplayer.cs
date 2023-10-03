using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Currently the only prefab relevant for this is the SimpleGearSlot - GearSlot1 is deprecated
public class MercGearDisplayer : BasicDisplayer
{
    public static MercGearDisplayer Instance;

    MercSheet mercSheet;



    [SerializeField]
    GameObject displayObject;

    [SerializeField]
    GearSlotDispayer[] gearSlots = new GearSlotDispayer[3]; // by EquipSlotType

    [SerializeField]
    StarGraph starGraph;
    [SerializeField]
    ExpBarDisplayer expBarDisplayer;
    
    [SerializeField]
    Image rightImage;
    [SerializeField]
    Image leftImage;

    [SerializeField]
    ClassEggPanel classEggPanel;



    public MercSheet GetMercSheet { get => mercSheet; }

    public System.Action OnMercChange;

    int current = 0;

    private void Awake()
    {
        //TBF TBD THIS IS LAZY
        Instance = this;
        current = 0;
        //gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        //SetMeFully(PlayerDataMaster.Instance.GetMercSheetByName());
        //SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(current)); // should actually look for the first AVAILABLE merc! TBF
    }

    public void TurnOn()
    {
        //SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(current)); // should actually look for the first AVAILABLE merc! TBF
        displayObject.SetActive(true);
    }
    public void TurnOff()
    {
        displayObject.SetActive(false);
    }

    public void SetMeFully(MercSheet ms)
    {
        mercSheet = ms;
        expBarDisplayer.SetBar(ms);
        Pawn p = ms.MyPawnPrefabRef<Pawn>();
        starGraph.SetToMerc(p._mercSheet);

        classEggPanel.SetEgg(ms.mercClass);
        //starGraph.SetToMerc(ms); //for some reason, this is the wrong one? FIX THIS! TBF - constructed MercSheets do NOT copy the pill profile correctly/at-all

        List<MercSheet> _sheets = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment> { MercAssignment.Available, MercAssignment.Room });

        current = _sheets.IndexOf(ms);

        int right = (current + 1 >= _sheets.Count) ? 0 : current + 1;
        int left = (current - 1 < 0) ? _sheets.Count - 1 : current - 1;
        //left and right image set:
        rightImage.sprite = _sheets[right].MyPawnPrefabRef<Pawn>().PortraitSprite;
        leftImage.sprite = _sheets[left].MyPawnPrefabRef<Pawn>().PortraitSprite;

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
        //if(current >= PlayerDataMaster.Instance.currentPlayerData.mercSheets.Count)
        List<MercSheet> _sheets = PlayerDataMaster.Instance.GetMercSheetsByAssignments(new List<MercAssignment> {MercAssignment.Available, MercAssignment.Room});
        if(current >= _sheets.Count)
        {
            current = 0;
        }
        if(current < 0)
        {
            current = _sheets.Count - 1;
        }
        SetMeFully(_sheets[current]); // should actually look for the first AVAILABLE merc! TBF

    }
}
