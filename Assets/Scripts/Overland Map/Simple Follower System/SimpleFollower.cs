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
    private float stoppingDistance;
    [SerializeField]
    BasicTimer basicTimer;

    Vector3 tavernPos => SiteMaster.Instance.tavernButton.transform.position;

    PathCreator myPath;

    /// <summary>
    /// When you Instantiate() a prefab with this component on it:
    /// It MUST be placed under the MapFollower canvas.
    /// You MUST position it manually (i.e. set its position somewhere near the tavern?).
    /// Then it would calculate its way to the destinationSite, taking {destinationSite.ETA} seconds to get there
    /// </summary>
    /// <param name="s"></param>
    /// <param name="destination"></param>
    //public void SetNewFollower(Squad s, SiteButton destination)
    //{
    //    destinationSite = destination;
    //    squad = s;
    //    groupPortrait.sprite = s.SquadPortrait;

    //    destinationSite.isWaitingForSquad = true;

    //    totalTravelTime = destinationSite.ETA;

    //    transform.position = tavernPos;


    //    StartCoroutine(nameof(WalkToTarget)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    //}
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

    /// <summary>
    /// Must already be Instantiated and positioned near tavern
    /// </summary>
    /// <param name="s"></param>
    /// <param name="destination"></param>
    /// <param name="departTime"></param>
    //public void SetLoadedFollower(Squad s, SiteButton destination, DateTime departTime)
    //{
    //    destinationSite = destination;
    //    squad = s;
    //    groupPortrait.sprite = s.SquadPortrait;

    //    destinationSite.isWaitingForSquad = true;
    //    //float delta = (float)(DateTime.Now - departTime).TotalSeconds - delayBeforeStart; //offsets whatever starting delay there would be
    //    float delta = (float)(DateTime.Now - departTime).TotalSeconds;

    //    if(delta >= destinationSite.ETA)
    //    {
    //        //place at site, and call arrive
    //        //transform.position = destinationSite.transform.position + (transform.position - destinationSite.transform.position)*stoppingDistance;
    //        transform.position = destinationSite.transform.position + (tavernPos - destinationSite.transform.position).normalized*stoppingDistance;
    //        Arrived();
    //        return;
    //    }
    //    //implied else (reutrn above)
        
    //    totalTravelTime = destinationSite.ETA - delta;

    //    //advance to new starting position
    //    transform.position = tavernPos + (destinationSite.transform.position - tavernPos) *(1- (destinationSite.ETA- delta)/destination.ETA);

    //    StartCoroutine(nameof(WalkToTarget)); //better to start without delay, and ADD delay by yeilding in coroutine. making sure it runs, so it's easier to stop 
    //}
    public void SetLoadedFollowerWithPath(Squad s, SiteButton destination, DateTime departTime)
    {
        destinationSite = destination;
        squad = s;
        groupPortrait.sprite = s.SquadPortrait;

        myPath = destination.pathCreator;

        destinationSite.isWaitingForSquad = true;
        //float delta = (float)(DateTime.Now - departTime).TotalSeconds - delayBeforeStart; //offsets whatever starting delay there would be
        _timeTraveled = (float)(DateTime.Now - departTime).TotalSeconds;

        if(_timeTraveled >= destinationSite.ETA)
        {
            //place at site, and call arrive
            //transform.position = destinationSite.transform.position + (transform.position - destinationSite.transform.position)*stoppingDistance;
            //transform.position = destinationSite.transform.position + (tavernPos - destinationSite.transform.position).normalized*stoppingDistance;
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

    /// <summary>
    /// Walks to destination from current position! (already placed on map!)
    /// </summary>
    /// <returns></returns>
    //IEnumerator WalkToTarget()
    //{
    //    //Delay before Start walking ?
    //    basicTimer.SetMe(totalTravelTime);
    //    yield return new WaitForSeconds(delayBeforeStart);

    //    Vector3 startPos = transform.position; //aka tavernPos
    //    float t = 0;
    //    if(destinationSite)
    //    {
    //        while (Vector3.Distance(transform.position, destinationSite.transform.position) > stoppingDistance)
    //        {
    //            yield return new WaitForFixedUpdate(); //caps follower movement and removes need for *Time.deltaTime
    //            t += Time.fixedDeltaTime;
    //            transform.position = Vector3.Lerp(startPos, destinationSite.transform.position, t/totalTravelTime);
    //        }
    //        Arrived();
    //    }
    //    else
    //    {
    //        Debug.LogError("no destination site for this follower: " + name);
    //    }
    //}
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

    void Arrived()
    {
        Debug.Log("follower arrived");
        destinationSite.SetArrivedSquad(squad); //also removes from away squads
        idleLogMessage = IdleLog.Instance.RecieveNewMessageWithSiteRef(new List<string> { squad.squadName, destinationSite.levelSO.name }, new List<Sprite> { groupPortrait.sprite }, destinationSite);
    }

    public void OnMyClick()
    {
        CancelExpeditionWindow.Instance.SetMe(this);
    }


    public void CancleMe()
    {
        PartyMaster.Instance.squads.Add(squad);

        squad.isAvailable = true;
       
        destinationSite.UnSetArrivingSquad();
        if(idleLogMessage)
        Destroy(idleLogMessage.gameObject);
        IdleLog.Instance.CloseIfEmptyCheck(1);


        StopAllCoroutines();
        Destroy(gameObject);
    }
}
