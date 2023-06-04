using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipArrowToggle : MonoBehaviour
{
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    GameObject upArrow;
    [SerializeField]
    GameObject downArrow;

    public void OnToggleChanged()
    {
        upArrow.SetActive(toggle.isOn);
        downArrow.SetActive(!toggle.isOn);
    }
}
