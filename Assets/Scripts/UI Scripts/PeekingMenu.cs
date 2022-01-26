using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeekingMenu : MonoBehaviour
{
    //RectTransform rectTransform;
    //Rect rect;
    //Vector3 originalPos;
    ////[SerializeField]
    ////float peekSpeed;
    ////[SerializeField]
    ////float hideSpeed;

    //[SerializeField]
    //float moveAwayAmount;

    //[SerializeField]
    //Sprite openSprite;
    //[SerializeField]
    //Sprite closedSprite;

    public Image sr;

    public bool menuOpen = false;

    [SerializeField]
    Button peekButton;
    [SerializeField]
    Button hideButton;


    public Animator anim; //temp set in instpector

    //void Start()
    //{
    //    //rectTransform = GetComponent<RectTransform>();
    //    //originalPos = rectTransform.position;


    //}
    public void HideMenu()
    {
        //StartCoroutine("MenuSlideClose");
        anim.SetTrigger("Close");
        menuOpen = false;
    }
    public void ShowMenu()
    {
        //StartCoroutine("MenuSlideOpen");
        anim.SetTrigger("Open");
        menuOpen = true;
    }

    //IEnumerator MenuSlideClose()
    //{
    //    StopCoroutine(MenuSlideOpen());
    //     menuOpen = true;

    //    Vector3 newScale = Vector3.up;

    //    while (menuOpen)
    //    {
    //        //rectTransform.Translate(Vector3.down * hideSpeed * Time.deltaTime);
    //        //rectTransform+= hideSpeed * Time.deltaTime;
    //        transform.localScale -= newScale * hideSpeed * Time.deltaTime; 
    //        if (transform.localScale.y <= .5f)
    //        {
    //            //sr.sprite = closedSprite;
    //            menuOpen = false; //and/or break
    //            break; //decided to also break
               

    //        }
    //        yield return null;
    //    }
    //}
    //IEnumerator MenuSlideOpen()
    //{
    //    StopCoroutine(MenuSlideClose());
    //    Vector3 newScale = Vector3.down;
    //    while (!menuOpen)
    //    {
    //        transform.localScale -= newScale * hideSpeed * Time.deltaTime;
    //        if (transform.localScale.y >= 1f)
    //        {
    //            menuOpen = true; //or break
    //            break; //decided to also break
    //            //sr.sprite = openSprite;
    //            //transform.localScale = Vector3.one;

    //        }
    //        yield return null;
    //    }
    //}

    public void ToggleMenu()
    {
        //if(menuOpen)
        //{
        //    StartCoroutine(MenuSlideClose());
        //}
        //else
        //{
        //    StartCoroutine(MenuSlideOpen());
        //}
        if (menuOpen)
            anim.SetTrigger("Close");
        else
            anim.SetTrigger("Open");

        menuOpen = !menuOpen;
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
