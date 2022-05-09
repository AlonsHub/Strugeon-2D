using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SiteData
{
    //transfer cooldown and ALL other SiteData related properites from SiteButton to SiteData - SiteButton is tricky though!
    //TBF 09/05/22

    public LevelEnum siteName; //level/site decide!!!

    public float logicalDistance; //for rings
    //cooldown
    //timeToArrive

    //siteState - idle/live/isSet, waiting for squad, squad in position

    //readiedsquad // squadOTW

    //bool isThereSquadOTW

    //squadOTW ETA/remainning travel time

    //mythings:
    //mySiteButton - should still do some things, but most data should be here - MAYBE this should ref nothing, and just be used as data?!
    //sitedisplyer should access this(siteData) essentially, and also it should be simplified to be a basicDisplayer variant TBF
    //
}
