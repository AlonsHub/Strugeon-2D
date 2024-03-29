﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TravelLog : MonoBehaviour
{
    public static TravelLog Instance;
    [SerializeField]
    List<SiteButton> sites; //NOT PUBLIC!
    //public SiteButton GetSiteButtonByIndex()
    //{

    //}

    [SerializeField]
    GameObject followerPrefab;
    [SerializeField]
    Transform followerCanvas;
    void Start()
    {
        if(Instance!= null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //DontDestroyOnLoad(gameObject); //not sure if needed;
        //if(sites == null || sites.Count==0)
        //sites = FindObjectsOfType<SiteButton>().ToList(); //TBF AF NOW

        Invoke(nameof(LateStart), .2f);
    }

    void LateStart()
    {
        if(PlayerDataMaster.Instance.currentPlayerData.squadSitesAndTimesRemainning.Count>0)
        {
            foreach (var item in PlayerDataMaster.Instance.currentPlayerData.squadSitesAndTimesRemainning)
            {
                //SquadFollower sf = Instantiate(followerPrefab, followerCanvas).GetComponent<SquadFollower>();
                Squad temp = PartyMaster.Instance.awaySquads.Where(x => x.roomNumber == item.squadIndex).FirstOrDefault();
                if (temp == null)
                    return;

                SimpleFollower sf = Instantiate(followerPrefab, followerCanvas).GetComponent<SimpleFollower>();
                //Squad temp = PartyMaster.Instance.squads.Where(x => x.roomNumber == item.squadIndex).FirstOrDefault();
                //sf.SetLoadedFollower(temp, sites[(int)item.siteEnum], System.DateTime.Parse(item.timeOfDeparture));
                sf.SetLoadedFollowerWithPath(temp, sites[(int)item.siteEnum], System.DateTime.Parse(item.timeOfDeparture));
            }
        }
    }

    
}
