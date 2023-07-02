using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGoldDisplayer : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text goldText;

    private void Start()
    {
        UpdateGold();
        Inventory.Instance.OnGoldChanged += UpdateGold;
    }

    //public void OnEnable()
    //{
    //}
    public void OnDestroy()
    {
        Inventory.Instance.OnGoldChanged -= UpdateGold;
    }


    void UpdateGold()
    {
        goldText.text = Inventory.Instance.Gold.ToString();
    }
}
