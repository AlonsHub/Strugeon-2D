using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BasicTimer : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;
    float seconds;
    public void SetMe(float secs)
    {
        seconds = secs;
        StopCoroutine(nameof(ClockCoroutine));
        StartCoroutine(nameof(ClockCoroutine));
    }

    IEnumerator ClockCoroutine()
    {
        while (seconds+1 >= 0f)
        {
            yield return new WaitForFixedUpdate();
            seconds -= Time.fixedDeltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(seconds+1f);
            text.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        }
    }
}
