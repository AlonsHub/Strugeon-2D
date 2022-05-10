using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventoryWindow : MonoBehaviour
{
    private void OnEnable()
    {
        Tavern.Instance.DisableWindowTier1(name);
    }

}
