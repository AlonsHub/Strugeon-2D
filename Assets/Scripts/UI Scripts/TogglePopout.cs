using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePopout : MonoBehaviour
{
    [SerializeField]
    float peekDuration;
    [SerializeField]
    float hideDuration;

    [SerializeField]
    float moveUpAmount;

    //[SerializeField]
    //Sprite openSprite;
    //[SerializeField]
    //Sprite closedSprite;

    //public Image sr;

    bool isOpen = false;

   //public void Toggle()
   // {
   //     isOpen = !isOpen;

   //     if(isOpen)
   //     {
   //         //openning
   //         StartCoroutine(Open());
   //     }
   //     else
   //     {
   //         //closing
   //         StartCoroutine(Close());
   //     }
   // }
    public void Toggle(bool isOn)
    {
        isOpen = isOn;

        if(isOpen)
        {
            //openning
            StartCoroutine(Open());
        }
        else
        {
            //closing
            StartCoroutine(Close());
        }
    }

    IEnumerator Close()
    {
        //StopAllCoroutines();
        StopCoroutine(Open());

        float t = 0;
        Vector3 newPos = transform.position - Vector3.up * moveUpAmount;
        Vector3 ogPos = transform.position;
        while (t <= hideDuration)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, t/hideDuration); //not ogPos, for expo
            yield return new WaitForSeconds(.005f);
            t += .005f;
        }
    }
    IEnumerator Open()
    {
        StopCoroutine(Close());
        //StopAllCoroutines();


        float t = 0;
        Vector3 newPos = transform.position + Vector3.up * moveUpAmount;
        Vector3 ogPos = transform.position;
        while (t <= hideDuration)
        {
            transform.position = Vector3.Lerp(transform.position, newPos, t / hideDuration); //not ogPos, for expo
            yield return new WaitForSeconds(.005f);
            t += .005f;
        }
    }

}
