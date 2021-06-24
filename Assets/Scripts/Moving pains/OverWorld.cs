using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverWorld : MonoBehaviour
{
    public List<Expedition> expeditions;
    public GameObject expeditionPrefab; //this has, and is controlled by, an ExpeditionFollower

    public Transform spawTrans;

    public static OverWorld Instance;

    public SiteButton homeSite;
    public SiteButton _selectedSite;


    private void Awake()
    {
        Instance = this;
    }
    //public void CreateExpedition()
    //{
    //    //create expedition follower
    //    //read current party from partymaster
    //    List<Pawn> _party = PartyMaster.Instance.currentMercParty;
    //    if(_party.Count == 0)
    //    {
    //        Debug.Log("NO PARTY SELECTED!");
    //        return;
    //    }

    //    GameObject go = Instantiate(expeditionPrefab, spawTrans.position, Quaternion.identity);
    //    go.transform.SetParent(GetComponentInParent<Canvas>().transform);
    //    ExpeditionFollower expeditionFollower = go.GetComponent<ExpeditionFollower>();
    //    expeditionFollower.pathCreator = _selectedSite.pathCreator;
    //    //expeditionFollower.levelSO = _selectedSite.levelSO; //KWAKWA DELA OMG FIX 
    //    expeditionFollower.destinationSiteButton = _selectedSite; //KWAKWA DELA OMG FIX 


    //    expeditions.Add( expeditionFollower.expedition = new Expedition(_selectedSite, _selectedSite.pathCreator, _party.ToList()));

    //    PartyMaster.Instance.currentMercParty.Clear();
    //    RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

    //    //init it's expedition manually RIGHT HERE!
    //}
    //public void CreateExpedition(SiteButton sb)
    //{
    //    //create expedition follower
    //    //read current party from partymaster
    //    List<Pawn> _party = PartyMaster.Instance.currentMercParty;
    //    if (_party.Count == 0)
    //    {
    //        Debug.Log("NO PARTY SELECTED!");
    //        return;
    //    }

    //    GameObject go = Instantiate(expeditionPrefab, spawTrans.position, Quaternion.identity);
    //    go.transform.SetParent(GetComponentInParent<Canvas>().transform);

    //    ExpeditionFollower expeditionFollower = go.GetComponent<ExpeditionFollower>();
    //    expeditionFollower.pathCreator = sb.pathCreator;
    //    //expeditionFollower.levelSO = sb.levelSO; //KWAKWA DELA OMG FIX 
    //    expeditionFollower.destinationSiteButton = sb; //KWAKWA DELA OMG FIX 


    //    expeditions.Add(expeditionFollower.expedition = new Expedition(sb, sb.pathCreator, _party.ToList()));

    //    PartyMaster.Instance.currentMercParty.Clear();
    //    RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

    //    //init it's expedition manually RIGHT HERE!
    //}

    public void CreateExpedition()
    {
        //create expedition follower
        //read current party from partymaster
        List<Pawn> _party = PartyMaster.Instance.currentMercParty;
        if (_party.Count == 0)
        {
            Debug.Log("NO PARTY SELECTED!");
            return;
        }

        MapWalker mw = Instantiate(expeditionPrefab, spawTrans.position, Quaternion.identity).GetComponent<MapWalker>();
        mw.transform.SetParent(GetComponentInParent<Canvas>().transform);
        mw.Init(spawTrans, _selectedSite.transform, .1f, _selectedSite.ETA);


       // expeditions.Add(expeditionFollower.expedition = new Expedition(sb, sb.pathCreator, _party.ToList()));

        PartyMaster.Instance.currentMercParty.Clear();
        RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

        //init it's expedition manually RIGHT HERE!
    }

    public void PrintExpeditions()
    {

    }
}
