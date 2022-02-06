using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PathCreation;

public class SiteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Dictionary<string, float> SiteCooldowns; // on cooldown as long as the value is greater than 0

    //SelectionScreenDisplayer displayer;
    [SerializeField]
    SiteDisplayer displayer;

    //public Level level; 
    public LevelSO levelSO;
    [Tooltip("Estiamted traveling duration in seconds")]
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
    public bool isWaitingForSquad;

    public PathCreator pathCreator; //path to site

    public Squad readiedSquad;
    public bool isReady = false;
    public bool isSet = false; //set means all level data has been set and hadn't been "used" yet. 
                               //Entering a site makes it !set, meaning it should be set again (i.e. re-set)


    [SerializeField]
    GameObject squadPickerObject; //turns-on on click - should be move here as it is now serialized in the OnClick event of SiteButtons

    private void Awake()
    {
        if (SiteCooldowns == null)
            SiteCooldowns = new Dictionary<string, float>();
    }
    private void Start()
    {
        //read if any site cooldown times exist

        if (PlayerDataMaster.Instance.SavedCooldowns.ContainsKey(levelSO.name))
        {
            if (PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] < maxCooldown)
            {
                timer = SiteCooldowns[levelSO.name] = PlayerDataMaster.Instance.SavedCooldowns[levelSO.name];
                isCooldown = true;
                
            }
            else
            {
                isCooldown = false; ///NOT ALWAYS
                timer = SiteCooldowns[levelSO.name] = maxCooldown;
            }
        }
        else
        {
            timer = maxCooldown;
        }
        //else
        //{
        //        isCooldown = false; ///NOT ALWAYS
        //        SiteCooldowns.Add(levelSO.name, maxCooldown);/// NOT ALWAYS

        //    ////////////////////////////// PROBLEMMMM!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
        //}

        Invoke("LateStart", .2f); //give data displayers time to find themselves with their own starts/awakes - this sucks
    }

    void LateStart()
    {
        if (myDataDisplay)
            myDataDisplay.SetActive(false);

        if (isCooldown)
            StartCooldownCaller();

    }

    public void GetProperties() //for filling displayers?
    {

    }
    public void OnClick()
    {
        if (isCooldown || isWaitingForSquad)
        {
            Debug.LogError("isCooldown or isWaitingForSquad");
            return;
        }

        if (isReady)
        {
            SendToArena();
            return;
        }
        
        //Debug.Log("Irrelevant click");
        //displayer.SetMe(this);
        //SiteDisplayer.SetActiveToAllInstances(false); //SiteDisplayers should do that on their own
        //myDataDisplay.SetActive(true);

        ////THIS WHOLE SECTION NEEDS REVISITING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        ////OverWorld.Instance._selectedSite = this; // not sure if this is the right way to do it
        ////LevelRef.Instance.siteToCooldown = this;
        ////LevelRef.Instance.visitedSiteName = name;

        /////
        ///// Check first if site is initiated
        ///// if not, randomize
        /////
        //if (!isSet)
        //{
        //    int rndDifficulty = Random.Range(0, 3);

        //    levelSO.levelData.SetLevelData((LairDifficulty)rndDifficulty);

        //    isSet = true;
        //}



        //displayer.SetMe(this);

    }


    void SendToArena()
    {
        OverWorld.Instance._selectedSite = this; // not sure if this is the right way to do it
        LevelRef.Instance.visitedSiteName = name; // not sure if this is the right way to do it (pretty sure it's not)
        LevelRef.Instance.SetCurrentLevel(levelSO);

        PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] = 0;

        PartyMaster.Instance.currentSquad = readiedSquad;

        SceneManager.LoadScene("ArenaSceneGeneric");
    }


    public void StartCooldownCaller()
    {
        StartCoroutine("StartCooldown");
    }
    float timer;

    IEnumerator StartCooldown()
    {
        isCooldown = true;

        //timer =  PlayerDataMaster.Instance.currentPlayerData.SiteCooldownTimes[levelSO.name];
        //if (PlayerDataMaster.Instance.currentPlayerData.SiteCooldownTimes[levelSO.name] > 0)
        //{

        //}
        //else
        //{
        //    timer = 0f;
        //}
        clockAndTimerParent.SetActive(true);
        timeText.text = "00:" + ((int)((maxCooldown / 60) - (int)timer / 60)).ToString("00") + ":" + ((int)maxCooldown - (int)(timer - (int)timer / 60));


        while (timer <= maxCooldown)
        {
            yield return new WaitForSecondsRealtime(clockDelay);
            timer += clockDelay;
            int secondsLeft = (int)maxCooldown - (int)timer;  
            timeText.text = "00:" + (secondsLeft/60).ToString("00") + ":" + (secondsLeft - (secondsLeft/60)*60).ToString("00");
        }
        clockAndTimerParent.SetActive(false);
        PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] = maxCooldown; //have a seperate method for AddCooldown that null checks and everything
        isCooldown = false;
    }

    private void OnDisable()
    {
        if (timer < maxCooldown)
        {
            PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] = timer; //have a seperate method for AddCooldown that null checks and everything
        }

        
    }

    public void UnSetMe() //for cancelExpedition cases
    {
        isReady = false;
        isWaitingForSquad = false;
        readiedSquad = null;
    }
    public void SetArrivedSquad(Squad s) //for cancelExpedition cases
    {
        isReady = true;
        isWaitingForSquad = false; 
        readiedSquad = s;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //nothing should have happened, but could be that site-info was open - and then squad arrived... should maybe close site data displayer?
        if (isCooldown || isWaitingForSquad || isReady)
        {
            Debug.LogError("the site is not available to disable display info for");
            return;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCooldown || isWaitingForSquad || isReady)
        {
            Debug.LogError("isCooldown or isWaitingForSquad or ready");
            return;
        }
        SiteDisplayer.SetActiveToAllInstances(false); //SiteDisplayers should do that on their own
        myDataDisplay.SetActive(true);

        //THIS WHOLE SECTION NEEDS REVISITING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //OverWorld.Instance._selectedSite = this; // not sure if this is the right way to do it
        //LevelRef.Instance.siteToCooldown = this;
        //LevelRef.Instance.visitedSiteName = name;

        ///
        /// Check first if site is initiated
        /// if not, randomize
        ///
        if (!isSet)
        {
            int rndDifficulty = Random.Range(0, 3);

            levelSO.levelData.SetLevelData((LairDifficulty)rndDifficulty);

            isSet = true;
        }



        displayer.SetMe(this);

    }
}
