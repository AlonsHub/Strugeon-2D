using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SiteMaster : MonoBehaviour
{
    //Will eventually manage all site setting, resetting and whatever

    // currently keepts record of who is going where and when - will save squads that are on their way to a site ATM
    
    List<SiteButton> sites; //NOT PUBLIC!
    //public SiteButton GetSiteButtonByIndex()
    //{

    //}

    void Start()
    {
        sites = FindObjectsOfType<SiteButton>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
