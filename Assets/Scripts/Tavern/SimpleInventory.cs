using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    //parent
    [SerializeField]
    Transform inventoryParent;
    //prefab with bsicdisplayer
    [SerializeField]
    GameObject itemDisplayerPrefab;

    [SerializeField] //just to see them
    List<BasicDisplayer> displayers = new List<BasicDisplayer>();

    private void OnEnable()
    {
        RefreshInventory();
        Inventory.Instance.OnInventoryChange += RefreshInventory;

    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= RefreshInventory;

    }
    private void EnsureOneDisplayerPerMagicItem()
    {

        int diff = displayers.Count - Inventory.Instance.magicItemCount;
        //int diff = displayers.Count - Inventory.Instance.inventoryItems.Count;

        if (diff == 0)
            return;

        if (diff < 0)
        {
            for (int i = 0; i < diff * -1; i++) //*-1 since diff is < 0, but is still the difference
            {
                GameObject displayerObject = Instantiate(itemDisplayerPrefab, inventoryParent);
                displayers.Add(displayerObject.GetComponent<BasicDisplayer>());
            }
        }
        else
        {
            //for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            for (int i = Inventory.Instance.magicItemCount; i < displayers.Count; i++)
            {
                Destroy(displayers[i].gameObject); //just disable them dude... TBF
            }
            displayers.RemoveRange(Inventory.Instance.magicItemCount, diff);
        }
    }

    public void RefreshInventory()
    {

       
        EnsureOneDisplayerPerMagicItem();

        //Draw items
        for (int i = 0; i < Inventory.Instance.magicItemCount; i++)
        {
            if (!Inventory.Instance.inventoryItems[i].FetchSprite())
                continue;

            displayers[i].SetMe(new List<string> { }, new List<Sprite> { Inventory.Instance.inventoryItems[i].itemSprite });
        }

        //while (displayers.Count + empties.Count < 16 || (displayers.Count + empties.Count) % 4 != 0)
        //{
        //    empties.Add( Instantiate(emptyDisplayerPrefab, inventoryParent));
        //}

    }
}
