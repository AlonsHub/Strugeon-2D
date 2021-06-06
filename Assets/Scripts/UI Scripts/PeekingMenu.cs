using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    Sprite openSprite;
    [SerializeField]
    Sprite closedSprite;

    public Image sr;

    void Start()
    {
        rectTransform = sr.transform;
        originalPos = rectTransform.position;
    }


    IEnumerator MenuSlideClose()
    {
        StopCoroutine(MenuSlideOpen());
         menuOpen = true;
        while (menuOpen)
        {
            rectTransform.Translate(Vector3.down * hideSpeed * Time.deltaTime);
            if (rectTransform.position.y <= originalPos.y - moveAwayAmount)
            {
                menuOpen = false; //or break
                sr.sprite = closedSprite;


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
            rectTransform.Translate(Vector3.up * peekSpeed * Time.deltaTime);
            if (rectTransform.position.y >= originalPos.y)
            {
                menuOpen = true; //or break
                sr.sprite = openSprite;
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
