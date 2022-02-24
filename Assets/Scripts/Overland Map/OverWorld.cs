using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//this might be trash that needs to be deleted TBF
public class OverWorld : MonoBehaviour
{
    //public List<Expedition> expeditions;
    public GameObject expeditionPrefab; //this has, and is controlled by, an ExpeditionFollower

    public Transform spawTrans;

    public static OverWorld Instance;

    //public Transform homeTrans;
    public SiteButton _selectedSite;


    private void Awake()
    {
        Instance = this;
    }
   

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
        //mw.(spawTrans, _selectedSite, .1f, _selectedSite.ETA, _party);


        RefMaster.Instance.SetNewMercList(_party);

       // expeditions.Add(expeditionFollower.expedition = new Expedition(sb, sb.pathCreator, _party.ToList()));

        PartyMaster.Instance.currentMercParty.Clear();
        RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

        //init it's expedition manually RIGHT HERE!
    }
}
