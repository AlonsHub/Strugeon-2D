using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using PathCreation;
using UnityEngine.UI;

public class SiteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //[SerializeField]
    SelectionScreenDisplayer displayer;

    //public Level level; 
    public LevelSO levelSO;
    [Tooltip("Estiamted duration in minutes")]
    public float ETA; //estiamted duration in minutes
    //public PathCreator pathCreator;
    //public PathCreator pathCreatorReturn;

    public ArrivalPanel arrivedMissionPanel;
    public GameObject arrivedMissionIcon;
    [SerializeField]
    private Vector3 arrivedPanelOffset;

    public GameObject myDataDisplay;
    //   public GameObject levelPrefab;

    public GameObject buttons;

    public float maxCooldown;
    public float clockDelay; //time between clock ticks

    public GameObject clockAndTimerParent;
    public TMPro.TMP_Text timeText;

    public bool isCooldown;

    private void Start()
    {
        //displayer = RefMaster.Instance.selectionScreenDisplayer;

        //isCooldown = false;

        //if (myDataDisplay)
        //myDataDisplay.SetActive(false);

        Invoke("LateStart", 1);
    }

    void LateStart()
    {
        displayer = RefMaster.Instance.selectionScreenDisplayer;

        isCooldown = false;

        if (myDataDisplay)
            myDataDisplay.SetActive(false);
    }

    
    //public void OpenArrivedPartyPanel(List<Pawn> newParty) //
    //{
    //    //transform.GetChild(0).gameObject.SetActive(true);
    //    GameObject go = Instantiate(arrivedMissionPanel, transform.position + arrivedPanelOffset, transform.rotation);
    //    go.transform.SetParent(gameObject.GetComponentInParent<Canvas>().transform);
    //    go.transform.localPosition = transform.localPosition + arrivedPanelOffset;

    //    ArrivalPanel ap = go.GetComponent<ArrivalPanel>();

    //    foreach (var p in newParty)
    //    {
    //        GameObject icon = Instantiate(arrivedMissionIcon, ap.parent);
    //        icon.GetComponent<Image>().sprite = p.PortraitSprite;
    //    }

    //    ExpeditionButton expeditionButton = go.GetComponentInChildren<ExpeditionButton>();
    //    RecallExpedition recallExpedition = go.GetComponentInChildren<RecallExpedition>();

    //    // expeditionButton.SetUpButton(levelSO, p)
    //    expeditionButton.levelSO = levelSO;
    //    expeditionButton.partyPawns = newParty;

    //    recallExpedition.party = newParty;
    //    recallExpedition.siteButton = this;
    //    recallExpedition.arrivalPanelToDestroy = go;
    //}

    public void GetProperties() //for filling displayers?
    {

    }

    private void OnMouseOver()
    {
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myDataDisplay)
            myDataDisplay.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myDataDisplay)
            myDataDisplay.SetActive(true);
    }
    public void OnClick()
    {
        OverWorld.Instance._selectedSite = this; // not sure if this is the right way to do it
        displayer.Reposition(transform);
        //displayer.EnableAndSet(level);
        int rndDifficulty = Random.Range(0, 3);

        levelSO.levelData.SetLevelData((LairDifficulty)rndDifficulty);

        //NOT THE CORRECT PLACE FOR THIS!!

        isCooldown = true;
        StartCoroutine("StartCooldown");
        //NOT THE CORRECT PLACE FOR THIS!!
        
        displayer.EnableAndSet(levelSO);
        displayer.durationText.text = ETA.ToString();
    }
    public void CloseMenu()
    {
        displayer.DisableAndReset();
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        float timer = 0f;
        clockAndTimerParent.SetActive(true);
        timeText.text = "00:" + ((int)(maxCooldown / 60) - (int)timer / 60) + ":" + ((int)maxCooldown - (int)(timer - (int)timer / 60));

        while (timer <= maxCooldown)
        {
            yield return new WaitForSecondsRealtime(clockDelay);
            timer += clockDelay;
            timeText.text = "00:" + ((int)(maxCooldown/60) - (int)timer / 60) + ":" + ((int)maxCooldown - (int)(timer - (int)timer / 60));
        }
        clockAndTimerParent.SetActive(false);
        isCooldown = false;
    }
}
