using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadRoomDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject squadSlotPrefab;
    [SerializeField]
    TMP_Text infoText;
    [SerializeField]
    Image roomImage;
    [SerializeField]
    MercDataDisplayer mercDataDisplayer;

    BasicMercSlot[] squadSlots;
    public void SetMe(Room room)
    {
        squadSlots = new BasicMercSlot[room.size];
        for (int i = 0; i < squadSlots.Length; i++)
        {
            //squadSlots[i]
        }
    }

    public void ClickedMercSlot(BasicMercSlot mercSlot)
    {
        foreach (var slot in squadSlots)
        {
            slot.FrameToggle(slot.Equals(mercSlot));
        }
        SetMercDataDisplayer(mercSlot.merc);
    }
    void SetMercDataDisplayer(Pawn merc)
    {
        if (!mercDataDisplayer.gameObject.activeSelf)
            mercDataDisplayer.gameObject.SetActive(true);

        mercDataDisplayer.SetMe(merc);
    }
}
