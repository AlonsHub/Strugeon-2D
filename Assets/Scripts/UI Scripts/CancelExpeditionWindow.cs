using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelExpeditionWindow : MonoBehaviour
{
    //This always exists on overmap - there's only one, it disables on awake, and can be turned on by clicking a expedeitions Icon
    //Doing so would enable this windows, as well as set it up to cancel that one specific expedition
    public static CancelExpeditionWindow Instance;

    [SerializeField]
    GameObject portraitPrefab;
    [SerializeField]
    Transform portraitParent;

    SquadFollower squadFollower;
    [SerializeField]
    Vector2 offset ;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false);
    }

    public void SetMe(SquadFollower sf)
    {
        gameObject.SetActive(true);
        squadFollower = sf;

        Vector2 newPos = Vector2.zero;

        if (Input.mousePosition.x > Screen.width / 2)
        {
            newPos.x = Input.mousePosition.x - offset.x;
        }
        else
        {
            newPos.x = Input.mousePosition.x + offset.x;
        }

        if (Input.mousePosition.y > Screen.height / 2)
        {
            newPos.y = Input.mousePosition.y - offset.y;
        }
        else
        {
            newPos.y = Input.mousePosition.y + offset.y;
        }

        transform.position = newPos;

        foreach (var pawn in squadFollower.squad.pawns)
        {
            GameObject go = Instantiate(portraitPrefab, portraitParent);

            go.GetComponentInChildren<Image>().sprite = pawn.PortraitSprite;
        }
    }

    public void CancelExpediton()
    {
        //return squad to available squads/room(?)
        PartyMaster.Instance.squads.Add(squadFollower.squad);

        //destroy the follower object and unset the sitebutton completely 
        squadFollower.CancelMe();
        PlayerDataMaster.Instance.RemoveLoggedSquad(squadFollower.squad.roomNumber);
        //gameObject.SetActive
        gameObject.SetActive(false);
    }
    public void KeepGoing()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        for (int i = portraitParent.childCount-1; i >= 0; i--)
        {
            Destroy(portraitParent.GetChild(i).gameObject);
        }
    }
}
