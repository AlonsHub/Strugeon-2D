using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUpdater : MonoBehaviour
{
    [SerializeField]
    TMP_Text goldDisplater;
    public void Start()
    {
        RefreshGold();
    }

    public void RefreshGold()
    {
        goldDisplater.text = Inventory.Instance.Gold.ToString();
    }
}
