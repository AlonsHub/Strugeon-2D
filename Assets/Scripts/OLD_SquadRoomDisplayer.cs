using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Deprecated! TBDeleted
/// </summary>
public class OLD_SquadRoomDisplayer : MonoBehaviour
{
    public static OLD_SquadRoomDisplayer Instance;

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

    [SerializeField]
    RosterSlot[] squadSlots;

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
        DestroySquadSlots();
    }

    private void OnEnable()
    {
        Tavern.Instance.DisableWindowTier1(name);
    }

    void DestroySquadSlots()
    {
        if (squadSlots != null)
        {
            for (int i = squadSlots.Length - 1; i >= 0; i--)
            {
                Destroy(squadSlots[i].gameObject);
            }
            squadSlots = null;
        }
    }
    public void SetMe(Room room)
    {
        //DestroySquadSlots();
        //squadSlots = new RosterSlot[room.size];


        for (int i = 0; i < room.size; i++)
        {
            //squadSlots[i] = Instantiate(squadSlotPrefab, slotParent).GetComponent<RosterSlot>();
            //squadSlots[i] = this;
            if (room.squad != null) //if it is, just have to empty slot there
            {
                if (i < room.squad.pawns.Count)
                    squadSlots[i].SetMe(room.squad.pawns[i]);
                else
                    squadSlots[i].SetMe();
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

    //public void EditSquad()
    //{
    //    Tavern.Instance.EditActiveSquad();
    //    gameObject.SetActive(false);
    //}
}
