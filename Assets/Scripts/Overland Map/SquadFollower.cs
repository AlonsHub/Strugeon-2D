using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using PathCreation;
using UnityEngine.UI;

public class SquadFollower : MonoBehaviour, IPointerClickHandler
{
    public Squad squad;
    public TMP_Text timeText;
    float remainingTime;
    PathCreator path;
    [SerializeField]
    float timerRate = .2f;

    PathFollower pathFollower;

    SiteButton siteButton;

    [SerializeField]
    Image leaderPortrait; //leader is the first merc

    //private void Start()
    //{
    //    pathFollower = GetComponent<PathFollower>();
    //    //transform.GetChild(0).gameObject.SetActive(false); //kills GFX until set
    //}

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

        siteButton.isWaitingForSquad = true; //should be a handled by SiteButton itself

        pathFollower = GetComponent<PathFollower>();

        //transform.position = p.path.GetPointAtTime(0);

        pathFollower.speed = path.path.cumulativeLengthAtEachVertex[path.path.cumulativeLengthAtEachVertex.Length - 1]/ remainingTime; //remainning time is "total time" in this context



        remainingTime += 1; //THIS IS FOR DISPLAY PURPOSES ONLY - MAKES SURE THE LAST SECOND COUNTED DOWN IS FROM 1 TO 0 AND NOT 0

        pathFollower.pathCreator = path;

        //squad.isAvailable = false; // turn back on when/if they return //set by Squad.SetAssigment

        leaderPortrait.sprite = squad.pawns[0].PortraitSprite; //the first merc is the leader - consider making Leader a Squad property
        StartCoroutine(nameof(RunTimer));

    }
    public void SetMe(Squad s, SiteButton sb, DateTime departTime)
    {

        siteButton = sb;

        squad = s;


        remainingTime = sb.ETA - (float)(DateTime.Now - departTime).TotalSeconds; //set remainning to full time

        if(remainingTime >0)
        {
            //spwan at site already

        }
        path = sb.pathCreator;

        siteButton.isWaitingForSquad = true; //should be a handled by SiteButton itself

        pathFollower = GetComponent<PathFollower>();

        //transform.position = p.path.GetPointAtTime(0);

        //pathFollower.speed = path.path.cumulativeLengthAtEachVertex[path.path.cumulativeLengthAtEachVertex.Length - 1] / remainingTime; //remainning time is "total time" in this context
        pathFollower.speed = path.path.cumulativeLengthAtEachVertex[path.path.cumulativeLengthAtEachVertex.Length - 1] / sb.ETA; //eta is "total time" in this context

        remainingTime += 1; //THIS IS FOR DISPLAY PURPOSES ONLY - MAKES SURE THE LAST SECOND COUNTED DOWN IS FROM 1 TO 0 AND NOT 0
        pathFollower.SetDistanceTravelled(departTime, sb.ETA);



        pathFollower.pathCreator = path;

        //squad.isAvailable = false; // turn back on when/if they return //set by Squad.SetAssigment

        leaderPortrait.sprite = squad.pawns[0].PortraitSprite; //the first merc is the leader - consider making Leader a Squad property
        StartCoroutine(nameof(RunTimer));

    }
    public void Go()
    {

    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(.5f);
        transform.GetChild(0).gameObject.SetActive(true); //unkills GFX until set


        while (remainingTime >= 0)
        {

            TimeSpan ts = new TimeSpan(0, 0, (int)remainingTime); //WRONG
            timeText.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            yield return new WaitForSeconds(timerRate);
            remainingTime -= timerRate;
        }

        //enable site to send to arena
        //THIS SHOULD BE A METHOD in SITE BUTTON
        //siteButton.isReady = true;
        //siteButton.isWaitingForSquad = false; //should be a handled by SiteButton itself
        //siteButton.readiedSquad = squad;
        siteButton.SetArrivedSquad(squad);

        //PlayerDataMaster.Instance.RemoveLoggedSquad(squad.roomNumber); //only after being sent to site!!!

        IdleLog.Instance.RecieveNewMessageWithSiteRef(new List<string> { squad.pawns[0].Name + " Squad", siteButton.levelSO.name}, new List<Sprite> {squad.pawns[0].PortraitSprite}, siteButton);

        //indicate somehow - also, connect the relevant squad to the site itself? 
        //AVOID a system that would have problems locating the squad to load-in
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //cancel expedition window?
        CancelExpeditionWindow.Instance.SetMe(this);
    }

    public void CancelMe()
    {
        squad.isAvailable = true;
        //siteButton.isWaitingForSquad = false; //should be a handled by SiteButton itself
        //siteButton.readiedSquad = null; //?
        siteButton.UnSetMe();

        StopAllCoroutines();
        Destroy(gameObject);
    }
}
