using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;
    [SerializeField] //just to see them
    List<MagicItemDIsplayer> displayers = new List<MagicItemDIsplayer>();
    private void OnEnable()
    {
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < displayers.Count; i++)
        {
            displayers[i].SetItem(Inventory.Instance.magicItems[i]);
        }
    }

    private void EnsureOneDisplayerPerMagicItem()
    {
        int diff = displayers.Count - Inventory.Instance.magicItemCount;

        if (diff == 0)
            return;

        if (diff < 0)
        {
            for (int i = 0; i < diff * -1; i++) //*-1 since diff is < 0, but is still the difference
            {
                GameObject displayerObject = Instantiate(itemDisplayerPrefab, inventoryParent);
                displayers.Add(displayerObject.GetComponent<MagicItemDIsplayer>());
            }
        }
        else
        {
            for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject);
            }
            displayers.RemoveRange(Inventory.Instance.magicItemCount, diff);
        }
    }

    private void OnDisable()
    {

    }


}
