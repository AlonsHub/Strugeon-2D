using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGoldDisplayer : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text goldText;

    private void Awake()
    {
        UpdateGold();
    }

    public void OnEnable()
    {
        Inventory.Instance.OnGoldChanged += UpdateGold;
    }
    public void Disable()
    {
        Inventory.Instance.OnGoldChanged -= UpdateGold;
    }


    void UpdateGold()
    {
        goldText.text = Inventory.Instance.Gold.ToString();
    }
}
