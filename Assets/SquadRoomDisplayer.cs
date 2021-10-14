using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadRoomDisplayer : MonoBehaviour
{
    public static SquadRoomDisplayer Instance;

    [SerializeField]
    GameObject squadSlotPrefab;
    [SerializeField]
    Transform slotParent;

    [SerializeField]
    TMP_Text infoText;
    [SerializeField]
    Image roomImage;
    [SerializeField]
    MercDataDisplayer mercDataDisplayer;

    BasicMercSlot[] squadSlots;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (squadSlots != null)
        {
            for (int i = 0; i < squadSlots.Length; i++)
            {
                Destroy(squadSlots[i].gameObject);
            }
        }
    }
    public void SetMe(Room room)
    {

        squadSlots = new BasicMercSlot[room.size];
        for (int i = 0; i < squadSlots.Length; i++)
        {
            squadSlots[i] = Instantiate(squadSlotPrefab, slotParent).GetComponent<BasicMercSlot>();
            squadSlots[i].squadRoomDisplayer = this; 
            if (i < room.squad.pawns.Count)
            {
                squadSlots[i].SetMe(room.squad.pawns[i]);
            }
            else
            {
                squadSlots[i].SetMe();
            }
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

    public void EditSquad()
    {
        Tavern.Instance.EditActiveSquad();
        gameObject.SetActive(false);
    }
}
