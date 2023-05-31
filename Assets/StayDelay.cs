using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayDelay : MonoBehaviour
{
    
    float delayAmount = .5f;
    [SerializeField]
    GameObject gfxToToggle;

    private void OnEnable()
    {
        StartCoroutine(nameof(WaitToShowMe));
    }

    IEnumerator WaitToShowMe()
    {
        yield return new WaitForSeconds(delayAmount);
        gfxToToggle.SetActive(true);
    }

    private void OnDisable()
    {
        gfxToToggle.SetActive(false);
    }
}
