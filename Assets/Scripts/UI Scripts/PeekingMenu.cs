using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekingMenu : MonoBehaviour
{
    Transform rectTransform;
    Vector3 originalPos;
    [SerializeField]
    float peekSpeed;
    [SerializeField]
    float hideSpeed;

    [SerializeField]
    float moveAwayAmount;

    void Start()
    {
        rectTransform = GetComponent<Transform>();
        originalPos = rectTransform.position;
    }


    IEnumerator MenuSlideClose()
    {
        StopCoroutine(MenuSlideOpen());
         menuOpen = true;
        while (menuOpen)
        {
            rectTransform.Translate(Vector3.right * hideSpeed * Time.deltaTime);
            if (rectTransform.position.y >= originalPos.y + moveAwayAmount)
            {
                menuOpen = false; //or break
            }
            yield return null;
        }
    }
    bool menuOpen = true;
    IEnumerator MenuSlideOpen()
    {
        StopCoroutine(MenuSlideClose());
        
        while (!menuOpen)
        {
            rectTransform.Translate(Vector3.down * peekSpeed * Time.deltaTime);
            if (rectTransform.position.y <= originalPos.y)
            {
                menuOpen = true; //or break
            }
            yield return null;
        }
    }

    public void ToggleMenu()
    {
        if(menuOpen)
        {
            StartCoroutine(MenuSlideClose());
        }
        else
        {
            StartCoroutine(MenuSlideOpen());
        }
    }

    //private void OnMouseEnter()
    //{
    //    StartCoroutine(MenuSlideOpen());
    //}
    //private void OnMouseExit()
    //{
    //    StartCoroutine(MenuSlideClose());
    //}
}
