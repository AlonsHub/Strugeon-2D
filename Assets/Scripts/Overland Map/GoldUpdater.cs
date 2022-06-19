using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUpdater : MonoBehaviour
{
    public static GoldUpdater Instance;

    [SerializeField]
    TMP_Text goldDisplater;
    public void Start()
    {
        RefreshGold();
        Instance = this;
    }

    public void RefreshGold()
    {
        goldDisplater.text = Inventory.Instance.Gold.ToString();
    }

    private void OnDisable()
    {
        Instance = null;
    }
}
