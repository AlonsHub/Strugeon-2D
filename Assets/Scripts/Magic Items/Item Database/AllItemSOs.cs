using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class AllItemSOs : ScriptableObject
{
    [SerializeField] List<MagicItemSO> allItemSOs;
    public List<MagicItemSO> GetAllItemSOList { get => allItemSOs; }
    public int itemSOCount => allItemSOs.Count;
    public MagicItemSO itemSOByIndex(int index) => allItemSOs[index];


    //public void InitAllItems()
    //{
    //    foreach (var itemSO in allItemSOs)
    //    {
    //        if (!itemSO.magicItem.FetchSprite())
    //            Debug.LogError($"Sprite not found for: {itemSO.magicItem.magicItemName}, spriteName is: {itemSO.magicItem.spriteName}");
    //    }
    //}
}
