using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

//simple follower would rather be a UI based button for visual fx sake
public class SimpleFollower : MonoBehaviour
{
    BasicDisplayer idleLogMessage;
    SiteButton destinationSite;
    NewSiteButton newDestinationSite;
    public Squad squad;

    //timer stuff
    //[SerializeField]
    //float tick; //in seconds
    [SerializeField]
    float delayBeforeStart; //in seconds
    float totalTravelTime; //in seconds
    float _timeTraveled; //in seconds
    //The timer display should be its own seperate thing... like EXPDisplayer


    [SerializeField]
    Button button;
    //img for portrait sprites
    [SerializeField]
    Image groupPortrait; //could be leader, or an icon or whatever
    [SerializeField]
    SpriteRenderer portrait;
    [SerializeField]
    private float stoppingDistance;
    [SerializeField]
    BasicTimer basicTimer;

    Vector3 tavernPos => SiteMaster.Instance.tavernButton.transform.position;

    PathCreator myPath;

    public void SetNewFollowerWithPath(Squad s, SiteButton destination)
    {
        destinationSite = destination;
        squad = s;
        groupPortrait.sprite = s.SquadPortrait;

        myPath = destination.pathCreator;

        destinationSite.isWaitingForSquad = true;

        totalTravelTime = destinationSite.ETA;
        _timeTraveled = 0f;

        transform.position = destinationSite.pathCreator.path.GetPointAtTime(0f);


        StartCoroutine(nameof(WalkToTargetWithPath)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    }
     public void SetNewFollowerWithPath(Squad s, NewSiteButton destination)
    {
        newDestinationSite = destination;
        squad = s;
        portrait.sprite = s.SquadPortrait;

        myPath = destination.pathCreator;

        newDestinationSite.isWaitingForSquad = true;

        totalTravelTime = newDestinationSite.ETA;
        _timeTraveled = 0f;

        transform.position = newDestinationSite.pathCreator.path.GetPointAtTime(0f);


        StartCoroutine(nameof(WalkToNewTargetWithPath)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    }

    /// <summary>
    /// Must already be Instantiated and positioned near tavern
    /// </summary>
    /// <param name="s"></param>
    /// <param name="destination"></param>
    /// <param name="departTime"></param>

    public void SetLoadedFollowerWithPath(Squad s, SiteButton destination, DateTime departTime)
    {
        destinationSite = destination;
        squad = s;
        portrait.sprite = s.SquadPortrait;

        myPath = destination.pathCreator;

        destinationSite.isWaitingForSquad = true;
        //float delta = (float)(DateTime.Now - departTime).TotalSeconds - delayBeforeStart; //offsets whatever starting delay there would be
        _timeTraveled = (float)(DateTime.Now - departTime).TotalSeconds;

        if(_timeTraveled >= destinationSite.ETA)
        {
            transform.position = destination.pathCreator.path.GetPointAtTime(1f, EndOfPathInstruction.Stop);
            Arrived();
            return;
        }
        //implied else (reutrn above)
        
        totalTravelTime = destinationSite.ETA - _timeTraveled;

        transform.position = myPath.path.GetPointAtTime(_timeTraveled / destination.ETA);
        //advance to new starting position
        //transform.position = tavernPos + (destinationSite.transform.position - tavernPos) *(1-(destinationSite.ETA- delta)/destination.ETA);
        //transform.position = myPath.path.GetPointAtTime(1 - (destinationSite.ETA - delta) / destination.ETA);

        StartCoroutine(nameof(WalkToTargetWithPath)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    }
    public void SetLoadedFollowerWithPath(Squad s, NewSiteButton destination, DateTime departTime)
    {
        newDestinationSite = destination;
        squad = s;
        portrait.sprite = s.SquadPortrait;

        myPath = destination.pathCreator;

        newDestinationSite.isWaitingForSquad = true;
        //float delta = (float)(DateTime.Now - departTime).TotalSeconds - delayBeforeStart; //offsets whatever starting delay there would be
        _timeTraveled = (float)(DateTime.Now - departTime).TotalSeconds;

        if(_timeTraveled >= newDestinationSite.ETA)
        {
            transform.position = destination.pathCreator.path.GetPointAtTime(1f, EndOfPathInstruction.Stop);
            Arrived();
            return;
        }
        //implied else (reutrn above)
        
        totalTravelTime = newDestinationSite.ETA - _timeTraveled;

        transform.position = myPath.path.GetPointAtTime(_timeTraveled / destination.ETA);
        //advance to new starting position
        //transform.position = tavernPos + (destinationSite.transform.position - tavernPos) *(1-(destinationSite.ETA- delta)/destination.ETA);
        //transform.position = myPath.path.GetPointAtTime(1 - (destinationSite.ETA - delta) / destination.ETA);

        StartCoroutine(nameof(WalkToNewTargetWithPath)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    }

    IEnumerator WalkToTargetWithPath()
    {
        //Delay before Start walking ?
        basicTimer.SetMe(totalTravelTime);
        yield return new WaitForSeconds(delayBeforeStart);

        Vector3 startPos = transform.position; //aka tavernPos
        //float t = 0;
        if(destinationSite)
        {
            //while (Vector3.Distance(transform.position, destinationSite.transform.position) > stoppingDistance)
            while (_timeTraveled<=destinationSite.ETA)
            {
                yield return new WaitForFixedUpdate(); //caps follower movement and removes need for *Time.deltaTime
                _timeTraveled += Time.fixedDeltaTime;

                transform.position = myPath.path.GetPointAtTime(_timeTraveled/ destinationSite.ETA, EndOfPathInstruction.Stop);

                //transform.position = Vector3.Lerp(startPos, destinationSite.transform.position, t/totalTravelTime);
            }
            Arrived();
        }
        else
        {
            Debug.LogError("no destination site for this follower: " + name);
        }
    }
     IEnumerator WalkToNewTargetWithPath()
    {
        //Delay before Start walking ?
        basicTimer.SetMe(totalTravelTime);
        yield return new WaitForSeconds(delayBeforeStart);

        Vector3 startPos = transform.position; //aka tavernPos
        //float t = 0;
        if(newDestinationSite)
        {
            //while (Vector3.Distance(transform.position, destinationSite.transform.position) > stoppingDistance)
            while (_timeTraveled<= newDestinationSite.ETA)
            {
                yield return new WaitForFixedUpdate(); //caps follower movement and removes need for *Time.deltaTime
                _timeTraveled += Time.fixedDeltaTime;

                transform.position = myPath.path.GetPointAtTime(_timeTraveled/ newDestinationSite.ETA, EndOfPathInstruction.Stop);

                //transform.position = Vector3.Lerp(startPos, destinationSite.transform.position, t/totalTravelTime);
            }
            Arrived();
        }
        else
        {
            Debug.LogError("no destination site for this follower: " + name);
        }
    }

    void Arrived()
    {
        Debug.Log("follower arrived");
        newDestinationSite.SetArrivedSquad(squad); //also removes from away squads
        idleLogMessage = IdleLog.Instance.RecieveNewMessageWithSiteRef(new List<string> { squad.squadName, newDestinationSite.levelSO.name }, new List<Sprite> { portrait.sprite }, newDestinationSite);
    }

    public void OnMyClick()
    {
        CancelExpeditionWindow.Instance.SetMe(this);
    }


    public void CancleMe()
    {
        PartyMaster.Instance.awaySquads.Remove(squad);
        PartyMaster.Instance.squads.Add(squad);

        squad.isAvailable = true;

        newDestinationSite.UnSetArrivingSquad();
        if(idleLogMessage)
        Destroy(idleLogMessage.gameObject);
        IdleLog.Instance.CloseIfEmptyCheck(1);


        StopAllCoroutines();
        Destroy(gameObject);
    }
}
