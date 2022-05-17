using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSheetToggle : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Toggle toggle;
    private void OnEnable()
    {
        toggle.isOn = GoogleSheetMaster.DoSheet;
        toggle.onValueChanged.AddListener(OnValueChange);
    }

    void OnValueChange(bool currentState)
    {
        GoogleSheetMaster.DoSheet = currentState;
    }
}
