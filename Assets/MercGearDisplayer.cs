using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercGearDisplayer : BasicDisplayer
{
    [SerializeField]
    GearSlotDispayer[] gearSlots = new GearSlotDispayer[3]; // by EquipSlotType
    MercSheet mercSheet;

    private void OnEnable()
    {
        //SetMeFully(PlayerDataMaster.Instance.GetMercSheetByName());
        SetMeFully(PlayerDataMaster.Instance.GetMercSheetByIndex(0));
    }

    public void SetMeFully(MercSheet ms)
    {
        mercSheet = ms;
        base.SetMe(new List<string> {ms.characterName.ToString(), ms._level.ToString()}, new List<Sprite> {ms.MyPawnPrefabRef<Pawn>().FullPortraitSprite });
        DisplayGear();
        //foreach of that Mercs items - display them in their relevant slots
    }

    public void DisplayGear()
    {
        for (int i = 0; i < gearSlots.Length; i++)
        {
            IEquipable eq;
            if ((eq = mercSheet.gear.GetItemBySlot((EquipSlotType)i)) != null)
            {
                gearSlots[i].SetMeFull(eq as MagicItem);
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
    }
}
