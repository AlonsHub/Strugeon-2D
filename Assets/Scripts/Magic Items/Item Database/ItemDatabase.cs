using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



/// <summary>
/// A Mono-Ton that Doesn't-destroy-on-load, after a gamesave is loaded. 
/// On start it quickly loads ONLY relevant items, by checking all pools of items that may appear (e.g. in inventory, equipment and current rewards for sites?)
///
/// </summary>
public class ItemDatabase : MonoBehaviour
{
    //Items will be serialized in an organized list, in a special collection (probablly scriptableObject)
    [SerializeField]AllItemSOs allItemSOs;

    public MagicItemSO insoilMystica;

    //the special collection will hold: 
    //a list of ItemSOs, loaded manually (TBD TBF)
    //a list of Names/Keys/IDs for those ItemSOs (TBD TBF) 

    //methods to get items by all sorts of params?
    //methods to load items as to make them available eariler?
    //dictionaries?
    Dictionary<string, MagicItemSO> idToItemSO;

    /// <summary>
    /// Access to the private Instance.idToItemSO dictionary 
    /// </summary>
    public static Dictionary<string, MagicItemSO> IDtoItemSO => Instance.idToItemSO;


    public static ItemDatabase Instance;
    void Awake() //Game object exists in the first scene, BUT IS DISABLED! - enabled once a profile/save had been chosen?
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

       SetFullDataBase();
    }

    void SetFullDataBase()
    {
        if(!allItemSOs)
            return;

        if (allItemSOs.itemSOCount == 0)
            return;

        //allItemSOs.InitAllItems();

        //create the dictionary:
        idToItemSO = new Dictionary<string, MagicItemSO>();

        foreach (var itemSO in allItemSOs.GetAllItemSOList)
        {
            if(idToItemSO.ContainsKey(itemSO.magicItem.magicItemName) || idToItemSO.ContainsValue(itemSO))
            {
                Debug.LogError($"Duplicate key or value. Key (item name): {itemSO.magicItem.magicItemName}");
                continue;
            }
            idToItemSO.Add(itemSO.magicItem.magicItemName, itemSO);
        }

        Debug.Log("Item dataebase full-load completed successfully"); //will try and add a relevant-load (as opposed to full) TBF
    }
    public void AddItem(string s, MagicItemSO so)
    {
        idToItemSO.Add(s, so);
    }

   
}
