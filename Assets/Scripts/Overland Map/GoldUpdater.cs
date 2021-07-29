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
        goldDisplater.text = Inventory.Instance.Gold.ToString();
        //PlayerDataMaster.Instance.currentPlayerData.gold = Inventory.Instance.Gold;
    }
}
