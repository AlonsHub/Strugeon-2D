using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[System.Serializable]
public class Expedition 
{
    public List<Pawn> expeditionPawns;
    public DateTime leaveTime;
    public float duration;
    Transform[] path;
    public GameObject token;
    public float speed;

    public int currentPathIndex;
    public bool hasBegun;
    public bool hasArrived;
    PathCreator pathCreator;
    public SiteButton destinationSite;
    //public ExpeditionFollower follower;
    //public Expedition(float newDuration, List<Pawn> newPawns, Transform[] newPath)
    public Expedition(SiteButton newDestinationSite, PathCreator newPathCreator, List<Pawn> newPawns)
    {
        pathCreator = newPathCreator;
        hasArrived = false;
        leaveTime = DateTime.Now;
        expeditionPawns = newPawns;
        duration = newDestinationSite.ETA;
        //path = newPath;
        destinationSite = newDestinationSite;
        currentPathIndex = 0;
        speed = newPathCreator.path.length / duration;
        hasBegun = true;


        //token.transform.position = path[0].position;
    }
   

    
    public void UpdatePath() //run from outside by a MonoBehavior Displayer class
    {
        if(hasArrived)
        {
            Debug.LogError("Updating a path that hasArrive == true");
            return;
        }

        //calculate time between now and "leave time" - or just cache it onenable and go on with it - setting it ondisable?
        DateTime now = DateTime.Now;
        float difference = duration /(float)(now - leaveTime).TotalSeconds;

        //extract method
        pathCreator.path.GetPointAtTime(difference, EndOfPathInstruction.Stop);


        //extract method

    }



    
}
