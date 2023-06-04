using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoryByDropdown : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Dropdown dropdown;
    [SerializeField]
    GameObject sortableObject;
    [SerializeField]
    UnityEngine.UI.Toggle isLowToHigh;

    ISortableByDropdown sortableByDropdown;

    private void Start()
    {
        sortableByDropdown = sortableObject.GetComponent<ISortableByDropdown>();
    }

    public void OnValueChanged()
    {
        sortableByDropdown.SortThisOut(dropdown.value, isLowToHigh.isOn);
    }

}
