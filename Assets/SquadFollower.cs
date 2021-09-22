using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PathCreation;

public class SquadFollower : MonoBehaviour
{
    public Squad squad;
    public TMP_Text timeText;
    float remainingTime;
    PathCreator path;
    [SerializeField]
    float timerRate = .2f;

    PathFollower pathFollower;

    SiteButton siteButton;

    private void Start()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    //public void SetMe(Squad s, float t, PathCreator p)
    //{
    //    squad = s;
    //    remainingTime = t;
    //    path = p;

    //    pathFollower = GetComponent<PathFollower>();

    //    //transform.position = p.path.GetPointAtTime(0);

    //    pathFollower.speed = p.path.cumulativeLengthAtEachVertex[p.path.cumulativeLengthAtEachVertex.Length - 1]/t;

    //    remainingTime += 1; //THIS IS FOR DISPLAY PURPOSES ONLY - MAKES SURE THE LAST SECOND COUNTED DOWN IS FROM 1 TO 0 AND NOT 0

    //    pathFollower.pathCreator = path;

    //    StartCoroutine("RunTimer");

    //}
    public void SetMe(Squad s, SiteButton sb)
    {

        siteButton = sb;

        squad = s;
        remainingTime = sb.ETA; //set remainning to full time
        path = sb.pathCreator;

        pathFollower = GetComponent<PathFollower>();

        //transform.position = p.path.GetPointAtTime(0);

        pathFollower.speed = path.path.cumulativeLengthAtEachVertex[path.path.cumulativeLengthAtEachVertex.Length - 1]/ remainingTime; //remainning time is "total time" in this context

        remainingTime += 1; //THIS IS FOR DISPLAY PURPOSES ONLY - MAKES SURE THE LAST SECOND COUNTED DOWN IS FROM 1 TO 0 AND NOT 0

        pathFollower.pathCreator = path;

        squad.isAvailable = false; // turn back on when/if they return
        StartCoroutine("RunTimer");

    }

    public void Go()
    {

    }

    IEnumerator RunTimer()
    {
        while (remainingTime >= 0)
        {
            TimeSpan ts = new TimeSpan(0, (int)remainingTime/60, (int)remainingTime);
            timeText.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            yield return new WaitForSeconds(timerRate);
            remainingTime -= timerRate;
        }

        //enable site to send to arena
        siteButton.isReady = true;
        siteButton.readiedSquad = squad;
        //indicate somehow - also, connect the relevant squad to the site itself? 
        //AVOID a system that would have problems locating the squad to load-in
    }
}
