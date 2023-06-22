using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortByDropdown : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Dropdown dropdown;
    [SerializeField]
    GameObject sortableObject;
    [SerializeField]
    UnityEngine.UI.Toggle isLowToHigh;

    ISortableByDropdown sortableByDropdown;

    private void Awake()
    {
        sortableByDropdown = sortableObject.GetComponent<ISortableByDropdown>();
    }

    private void Start()
    {
        sortableByDropdown.SortThisOut(dropdown.value, isLowToHigh.isOn);
    }

    public void OnValueChanged()
    {
        sortableByDropdown.SortThisOut(dropdown.value, isLowToHigh.isOn);
    }

}
