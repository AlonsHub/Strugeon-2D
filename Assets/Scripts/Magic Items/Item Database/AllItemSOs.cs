using System.Collections;
//using System.IO;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class AllItemSOs : ScriptableObject
{
    [SerializeField] List<MagicItemSO> allItemSOs;
    public List<MagicItemSO> GetAllItemSOList { get => allItemSOs; }
    public int itemSOCount => allItemSOs.Count;
    public MagicItemSO itemSOByIndex(int index) => allItemSOs[index];

    //[ContextMenu("Print for Netta")]
    //public void PrintItemsAndValueForNetta()
    //{
    //    string path = Application.dataPath + "/ItemsAndValues.txt";

    //    StreamWriter sw = File.CreateText(path);

    //    foreach (var item in GetAllItemSOList)
    //    {
    //        sw.WriteLine($"{item.name} || Gold value: {item.magicItem.goldValue} || Bonus: {item.magicItem.ItemBenefitDescription()}");
    //        foreach (var element in item.magicItem.spectrumProfile.elements)
    //        {
    //            sw.WriteLine($"{element.nulColour}: {element.value}");
    //        }
    //        sw.WriteLine("___________________\n");
    //    }
    //    sw.Close();
    //}

    //public void InitAllItems()
    //{
    //    foreach (var itemSO in allItemSOs)
    //    {
    //        if (!itemSO.magicItem.FetchSprite())
    //            Debug.LogError($"Sprite not found for: {itemSO.magicItem.magicItemName}, spriteName is: {itemSO.magicItem.spriteName}");
    //    }
    //}
}
