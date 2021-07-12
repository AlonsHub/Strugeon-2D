using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;

public class ArrivalPanel : MonoBehaviour
{
    public List<Pawn> mercs;
    public List<Image> mercImgs;
    //public static ArrivalPanel Instance;
    //private void Awake()
    //{
    //    if(Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;

    //    gameObject.SetActive(false); //turns itself off after registration
    //}

    public void SetMe(List<Pawn> pawns)
    {
        gameObject.SetActive(true); //turns itself on after ______(?)
        for (int i = 0; i < pawns.Count; i++)
        {
            mercImgs[i].sprite = pawns[i].PortraitSprite;
        }
    }




}
