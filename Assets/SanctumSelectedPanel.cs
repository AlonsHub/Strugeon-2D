using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanctumSelectedPanel : BasicDisplayer
{
    public static SanctumSelectedPanel Instance;

    //displayer
    //BasicDisplayer basicDisplayer;

    public MagicItem magicItem;
    [SerializeField]
    MagicItem emptyItem;
    [SerializeField]
    StarGraph starGraphNoolBarPanel;

    [SerializeField]
    GameObject[] objectsToToggle;

    [SerializeField]
    UnityEngine.UI.Button inhaleButton;

    private void Awake() //TBF TBD LAZY
    {
        Instance = this;
        SetMeFull();
    }
    
    private void OnDisable()
    {
        Instance = null;
    }
    
    private void OnDestroy()
    {
        Instance = null;
    }

    public void SetMeFull(MagicItem item)
    {
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(true);
        }
        magicItem = item;
        inhaleButton.interactable = true;
        starGraphNoolBarPanel.SetToItem(item);
        base.SetMe(new List<string> { item.magicItemName, item._EquipSlotType().ToString(), item._Benefit().BenefitStatName(),item._Benefit().Value().ToString() ,item.goldValue.ToString(), item.ItemDescription(), item._Benefit().BenefitStatName()}, new List<Sprite> {item.itemSprite});
    }
    public void SetMeFull()
    {
        foreach (var obj in objectsToToggle)
        {
            obj.SetActive(false);
        }
        magicItem = null;
        inhaleButton.interactable = false;
        starGraphNoolBarPanel.SetToItem(emptyItem);
        base.SetMe(new List<string> { "", "", "", "", "", "", ""}, new List<Sprite> { emptyItem.itemSprite});
    }

}
