using System.Collections;
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
    //[SerializeField]
    //Transform tavernTrans;
    [SerializeField]
    Transform canvasTrans;
    [SerializeField]
    SiteButton tgtSite;
    SimpleSiteButton tgtSimpleSite;

    [SerializeField]
    GameObject clickOffMeButton; //this button needs to activate with the squad picker, but it is not a good fit to be it's child
                                 //so it comes on and off whenever this Enables/Disables

    //private void Start()
    //{
    //    gameObject.SetActive(false);
    //    //availavleSquads = PartyMaster.Instance.squads;

    //}
    bool _justEnabled;
    private void OnEnable()
    {
        //Refresh();
        clickOffMeButton.SetActive(true);

        foreach (var item in squadSlots)
        {
            item.UnSetMe();
        }

        _justEnabled = true;
    }
    public void Refresh()
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

        newPos.y = Mathf.Clamp(newPos.y, Screen.height / 4, Screen.height * 3 / 4);

        transform.position = newPos;

        squadToggler.RefreshSlots();


        int count = 0;
        foreach (var item in PlayerDataMaster.Instance.currentPlayerData.rooms)
        {
            if (!item.squad.isAvailable)
                continue;

            squadSlots[count].SetMe(item.squad);
            count++;
        }

       
        

    }
    public void Refresh(Transform centerOfData)
    {
        //int i = 0;
        Vector2 newPos = (Vector2)centerOfData.position + offset;

        if(centerOfData.position.x > Screen.width/2)
        {
            newPos.x = centerOfData.position.x - offset.x;
        }
        else
        {
            newPos.x = centerOfData.position.x + offset.x;
        }

        if (centerOfData.position.y > Screen.height / 2)
        {
            newPos.y = centerOfData.position.y - offset.y;
        }
        else
        {
            newPos.y = centerOfData.position.y + offset.y;
        }

        newPos.y = Mathf.Clamp(newPos.y, Screen.height / 4, Screen.height * 3 / 5);

        transform.position = newPos;

        squadToggler.RefreshSlots();



        //if (PartyMaster.Instance.squads == null /*|| PartyMaster.Instance.squads.Count == 0*/)
        //    return;

        ///Consider unsetting all slots before setting them to avoid the GhostPrint of unavailable squads

        //foreach (var item in squadSlots)
        //{
        //    item.UnSetMe();
        //}

        int count = 0;
        foreach (var item in PlayerDataMaster.Instance.currentPlayerData.rooms)
        {
            if (!item.squad.isAvailable)
                continue;

            squadSlots[count].SetMe(item.squad);
            count++;
        }


        if (_justEnabled)
        {
            _justEnabled = false;
            if (squadSlots[0].squad != null && squadSlots[0].squad.pawns != null && squadSlots[0].squad.pawns.Count != 0)
                squadSlots[0].SelectMe();
            //squadToggler.SelectIndex(0);
        }



    }

    private void OnDisable()
    {
        clickOffMeButton.SetActive(false);

        //foreach (var item in squadSlots)
        //{
        //    item.UnSetMe();
        //}

        SiteDisplayer.Instance.SetOnOff(false);

    }

    public void SendSquad() //called by button in inspector
    {
        //SquadPickerWindow is closed via serialized event listener (consider putting it into code instead)
        int index = squadToggler.SelectedIndex();
        Squad toSend = squadSlots[index].squad;

        //check if a squad is chosen:
        if (index == -1 || !tgtSite)
        {
            if(tgtSimpleSite)
            {
                tgtSimpleSite.SendToArena(toSend);
            }
            return;
        }


        GameObject go = Instantiate(followerPrefab, canvasTrans);
        
        go.GetComponent<SimpleFollower>().SetNewFollowerWithPath(toSend, tgtSite);


        toSend.SetMercsToAssignment(MercAssignment.AwaySquad, toSend.roomNumber); 

        PlayerDataMaster.Instance.LogSquadDeparture(toSend.roomNumber, tgtSite.siteData.siteName, System.DateTime.Now);

        squadSlots[index].UnSetMe(); //weird but good

    }

    public void SetSite(SiteButton sb) //CRITICAL TBF! this is called in inspector //FIXED!
    {
        tgtSite = sb;
    }
     public void SetSimpleSite(SimpleSiteButton ssb) //CRITICAL TBF! this is called in inspector //FIXED!
    {
        tgtSimpleSite = ssb;
    }

}
