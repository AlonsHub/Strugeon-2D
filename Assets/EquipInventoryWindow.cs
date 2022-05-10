using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventoryWindow : MonoBehaviour
{
    [SerializeField]
    TogglePopout myToggle;
    private void OnEnable()
    {
        Tavern.Instance.DisableWindowTier1(name);
        myToggle.Toggle(true);
    }

    private void OnDisable()
    {
         myToggle.Toggle(false);
    }
}
