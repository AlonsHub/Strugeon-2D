using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PathCreation;
using UnityEngine.UI;

public class SiteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    SelectionScreenDisplayer displayer;

    public Level level;
    public LevelSO levelSO;
    [Tooltip("Estiamted duration in minutes")]
    public float ETA; //estiamted duration in minutes
    public PathCreator pathCreator;
    public PathCreator pathCreatorReturn;

    public GameObject arrivedMissionPanel;
    public GameObject arrivedMissionIcon;
    [SerializeField]
    private Vector3 arrivedPanelOffset;

    public GameObject myDataDisplay;
    //   public GameObject levelPrefab;

    public GameObject buttons;

    //public void OnClick()
    //{
    //    GameMaster.Instance.LoadLevelSO()
    //    SceneManager.LoadScene(1);
    //}
    //private void Awake()
    //{
    //    transform.GetChild(0).gameObject.SetActive(false);
    //}
    private void Start()
    {
        if(myDataDisplay)
        myDataDisplay.SetActive(false);
    }
    public void OpenArrivedPartyPanel(List<Pawn> newParty) //
    {
        //transform.GetChild(0).gameObject.SetActive(true);
        GameObject go = Instantiate(arrivedMissionPanel, transform.position + arrivedPanelOffset, transform.rotation);
        go.transform.SetParent(gameObject.GetComponentInParent<Canvas>().transform);
        go.transform.localPosition = transform.localPosition + arrivedPanelOffset;

        ArrivalPanel ap = go.GetComponent<ArrivalPanel>();

        foreach (var p in newParty)
        {
            GameObject icon = Instantiate(arrivedMissionIcon, ap.parent);
            icon.GetComponent<Image>().sprite = p.PortraitSprite;
        }

        ExpeditionButton expeditionButton = go.GetComponentInChildren<ExpeditionButton>();
        RecallExpedition recallExpedition = go.GetComponentInChildren<RecallExpedition>();

        // expeditionButton.SetUpButton(levelSO, p)
        expeditionButton.levelSO = levelSO;
        expeditionButton.partyPawns = newParty;

        recallExpedition.party = newParty;
        recallExpedition.siteButton = this;
        recallExpedition.arrivalPanelToDestroy = go;
    }

    public void GetProperties() //for filling displayers?
    {

    }

    private void OnMouseOver()
    {
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //displayer.DisableAndReset();+
        if (myDataDisplay)
            myDataDisplay.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myDataDisplay)
            myDataDisplay.SetActive(true);
        //displayer.DisableAndReset();
        //displayer.Reposition(transform);
        //displayer.EnableAndSet(level);
        //Debug.Log("works");
    }
    public void OnClick()
    {
        OverWorld.Instance._selectedSite = this;
        displayer.Reposition(transform);
        displayer.EnableAndSet(level);
        displayer.durationText.text = ETA.ToString();

        
        //buttons.SetActive(true);
        //buttons.transform.SetParent(RefMaster.Instance.selectionScreenDisplayer.transform);
        //buttons.transform.SetAsLastSibling();
       // Debug.Log("works");
    }
    public void CloseMenu()
    {
        displayer.DisableAndReset();
    }
}
