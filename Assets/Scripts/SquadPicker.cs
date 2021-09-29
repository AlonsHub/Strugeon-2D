﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadPicker : MonoBehaviour
{
    public List<Squad> availavleSquads;
    public Transform[] rowParents;
    public SquadSlot[] squadSlots;

    //[SerializeField]
    //GameObject squadSlotPrefab;

    [SerializeField]
    GameObject portraitPrefab;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    SquadToggler squadToggler; //public?

    [SerializeField]
    GameObject followerPrefab;
    [SerializeField]
    Transform tavernTrans;
    [SerializeField]
    Transform canvasTrans;

    SiteButton tgtSite;
    private void Start()
    {
        gameObject.SetActive(false);
        //availavleSquads = PartyMaster.Instance.squads;

    }

    public void OnEnable()
    {
        //int i = 0;
        Vector2 newPos = Vector2.zero;

        if(Input.mousePosition.x > Screen.width/2)
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


      

        if (PartyMaster.Instance.squads == null /*|| PartyMaster.Instance.squads.Count == 0*/)
            return;
        int count = 0;
        foreach (var item in PartyMaster.Instance.squads)
        {
            if (!item.isAvailable)
                continue;

            squadSlots[count].SetMe(item);
            count++;
        }

       
        

    }

    private void OnDisable()
    {
        //foreach(Transform t in rowParents)
        //{
        //    if(t.childCount !=0)
        //    {
        //        for (int i = t.childCount-1; i >= 0; i--)
        //        {
        //            Destroy(t.GetChild(i).gameObject);
        //        }
        //    }
        //}
        foreach (var item in squadSlots)
        {
            item.UnSetMe();
        }
    }

    public void SendSquad()
    {
        //check if a squad is chosen:
        if(squadToggler.selectedToggle == -1 || !tgtSite)
        {
            Debug.LogError("No squad selected or target site!");
            return;
        }

        GameObject go = Instantiate(followerPrefab, canvasTrans);
        //go.transform.position = tavernTrans.position;
        //go.GetComponent<SquadFollower>().SetMe(PartyMaster.Instance.squads[squadToggler.selectedToggle], tgtSite.ETA, tgtSite.pathCreator);
        go.GetComponent<SquadFollower>().SetMe(squadSlots[squadToggler.selectedToggle].squad, tgtSite);
    }

    public void SetSite(SiteButton sb)
    {
        tgtSite = sb;
    }
}
