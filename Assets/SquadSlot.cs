using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadSlot : MonoBehaviour
{
    //this is an unusual type of Slot class - it starts gets re/set manuallyand; never really instantiated.

    [SerializeField]
    Image[] mercSlotImages_BGs;
    [SerializeField]
    Image[] mercSlotImages;
    [SerializeField]
    Sprite onFrameSprite;
    [SerializeField]
    Sprite offFrameSprite;

    public Squad squad;
    public bool isRelevant = false;
    public bool isSelected = false;
    void Start()
    {
        
    }

    public void SelectMe()
    {
        Debug.LogError("began");

        foreach (var item in mercSlotImages_BGs)
        {
            //Debug.LogError("turbn onsdfs ");
            item.sprite = onFrameSprite;
        }
    }
    public void DeSelectMe()
    {
        foreach (var item in mercSlotImages_BGs)
        {
            item.sprite = offFrameSprite;
        }
    }

    public void SetMe(Squad s)
    {
        squad = s;
        for (int i = 0; i < s.pawns.Count; i++)
        {
            mercSlotImages[i].sprite = s.pawns[i].PortraitSprite;
            mercSlotImages[i].color = Color.white;
        }
        isRelevant = true;
    }
    public void UnSetMe()
    {
        //squad = null;
        foreach (var item in mercSlotImages)
        {
            item.sprite = null;
            item.color = new Color(0, 0, 0, 0);
        }
    }
}
