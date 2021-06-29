//using PathCreation;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RecallExpedition : MonoBehaviour
//{
//    public SiteButton siteButton;
//    public List<Pawn> party;
//    public GameObject arrivalPanelToDestroy;

//    public GameObject recallPrefab;
//    public EndOfPathInstruction endOf;

    
//    public void Recall()
//    {
//        //if (!siteButton)
//        //    siteButton = GetComponentInParent<SiteButton>();
//        PartyMaster.Instance.availableMercs.AddRange(party);
//        RefMaster.Instance.selectionScreenDisplayer.RefreshMercDisplay();

//        GameObject go = Instantiate(recallPrefab, transform.position, Quaternion.identity);
//        go.transform.SetParent(GetComponentInParent<Canvas>().transform);
//        BackHomeExpedition expeditionFollower = go.GetComponent<BackHomeExpedition>();
//        expeditionFollower.pathCreator = siteButton.pathCreatorReturn;
//        go.transform.position = expeditionFollower.pathCreator.path.GetPointAtTime(0f, EndOfPathInstruction.Stop);
//        expeditionFollower.expedition = new Expedition(OverWorld.Instance.homeSite, siteButton.pathCreatorReturn, party);
//        Destroy(arrivalPanelToDestroy);

//        //OverWorld.Instance.
//    }
//}
