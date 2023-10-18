﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PathCreation;
using System;

public class NewSiteButton : MonoBehaviour
{
    //public static Dictionary<string, float> SiteCooldowns; // on cooldown as long as the value is greater than 0

    //SelectionScreenDisplayer displayer;
    [SerializeField]
    SiteDisplayer displayer;

    //public Level level; 
    public LevelSO levelSO;

    public SiteData siteData; //re work needed!

    //[Tooltip("Estiamted traveling duration in seconds")]
    public float ETA => siteData.ETA;

    public Transform dataDisplayCenterTrans; //maybe remove?

    private readonly int clockDelay = 1; //time between clock ticks //seeing how this works, it could honestly just be a string...

    public GameObject clockAndTimerParent;
    public TMPro.TMP_Text timeText;

    [HideInInspector]
    public bool isCooldown;
    [HideInInspector]
    public bool isWaitingForSquad;

    public PathCreator pathCreator; //path to site

    [HideInInspector]
    public Squad readiedSquad;
    [HideInInspector]
    public bool isReady = false;
    public bool isLevelDataSet { get => levelSO.levelData.isSet; set => levelSO.levelData.isSet = value; } //set means all level data has been set and hadn't been "used" yet. 
                                                                                                           //Entering a site makes it !set, meaning it should be set again (i.e. re-set)


    //[SerializeField]
    //GameObject squadPickerObject; //turns-on on click - should be move here as it is now serialized in the OnClick event of SiteButtons
    SquadPicker _squadPicker => SquadPicker.Instance; //turns-on on click - should be move here as it is now serialized in the OnClick event of SiteButtons

    public SpriteButton spriteButton;

    [SerializeField]
    LevelSO[] forestSos;
    [SerializeField]
    LevelSO[] darkForestSos;
    [SerializeField]
    LevelSO[] ruinsSos;
    [SerializeField]
    LevelSO[] swampSos;
    [SerializeField]
    LevelSO[] lakeSos;

    public LairDifficulty[] relevantDifficulties;

    static int[] counts = new int[5];
    [ContextMenu("ClearCounts")]
    public void ClearCounts()
    {
        for (int i = 0; i < counts.Length; i++)
        {
            counts[i] = 0;
        }
    }
    [ContextMenu("NameFix")]
    public void NameFix()
    {
        
        name = spriteButton.hoverSprite.name;
        switch (name)
        {
            case "Forest":
                name += $" {counts[0]+1}";
                levelSO = forestSos[counts[0]];
                forestSos[counts[0]].name = spriteButton.hoverSprite.name + $" {counts[0] + 1}";
                counts[0]++;
                break;
                case "DarkForest":
                name += $" {counts[1] + 1}";
                levelSO = darkForestSos[counts[1]];
                darkForestSos[counts[1]].name = spriteButton.hoverSprite.name + $" {counts[1] + 1}";
                counts[1]++;
                break;
                case "Ruins":
                name += $" {counts[2] + 1}";
                levelSO = ruinsSos[counts[2]];
                ruinsSos[counts[2]].name = spriteButton.hoverSprite.name + $" {counts[2] + 1}";
                counts[2]++;
                break;
                case "Swamp":
                name += $" {counts[3] + 1}";
                levelSO = swampSos[counts[3]];
                swampSos[counts[3]].name = spriteButton.hoverSprite.name + $" {counts[3] + 1}";
                counts[3]++;
                break;
                case "Lake":
                name += $" {counts[4] + 1}";
                levelSO = lakeSos[counts[4]];
                lakeSos[counts[4]].name = spriteButton.hoverSprite.name+ $" {counts[4] + 1}";
                counts[4]++;
                break;

            default:
                break;
        }
    }


    public LairDifficulty GetRandomRelevantDifficulty()
    {
        return relevantDifficulties[UnityEngine.Random.Range(0, relevantDifficulties.Length)];

    }

    private void Start()
    {
        siteData.logicalDistance = (Vector3.Distance(LoadTavernScene.tavernButtonTransform.position, transform.position));

        if(!RevealRing.Instance)
        {
            isCooldown = false;
            Invoke("LateStart", .2f); //give data displayers time to find themselves with their own starts/awakes - this sucks
            return;
        }

        //Reveal ring check
        if (!RevealRing.Instance.IsSiteInRing(siteData))
        {
            levelSO.levelData.isSet = false;
            gameObject.SetActive(false);
            return;
        }

        if (PlayerDataMaster.Instance.SiteCooldowns.ContainsKey(levelSO.name) && PlayerDataMaster.Instance.SiteCooldowns[levelSO.name].HasValue)
        {
            if (DateTime.Compare(PlayerDataMaster.Instance.SiteCooldowns[levelSO.name].Value, DateTime.Now) <= 0) // -1 if cooldown date is earlier than now (i.e. passed date, cooldown is off), 0 is equal (unlikely)
            {
                isCooldown = false;
                timerSpan = TimeSpan.Zero;
                PlayerDataMaster.Instance.ClearSiteCooldown(levelSO.name);

            }
            else
            {
                timerSpan = PlayerDataMaster.Instance.SiteCooldowns[levelSO.name].Value - DateTime.Now;
                isCooldown = true;

            }
        }
        else
        {
            //timer = maxCooldown;
            isCooldown = false;
            timerSpan = TimeSpan.Zero;
            PlayerDataMaster.Instance.ClearSiteCooldown(levelSO.name);//also adds it to the dict if it doesnt exist
        }


        Invoke("LateStart", .2f); //give data displayers time to find themselves with their own starts/awakes - this sucks
    }

    void LateStart()
    {
        if (isCooldown)
            StartCooldownCaller();
    }


    public void OnClick()
    {
        if (isCooldown || isWaitingForSquad || _squadPicker.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("isCooldown or isWaitingForSquad or squadpicker is active");
            return;
        }

        if (isReady)
        {
            SendToArena();
            return;
        }


        _squadPicker.gameObject.SetActive(true); //should really disable and then enable to get the respositioning OnEnable //DONE!
        _squadPicker.Refresh(transform); //chache this earlier! //cached as _squadPicker :)
        _squadPicker.SetSite(this);
    }


    void SendToArena()
    {
        PlayerDataMaster.Instance.RemoveLoggedSquad(readiedSquad.roomNumber);
        PartyMaster.Instance.awaySquads.Remove(readiedSquad);

        //OverWorld.Instance._selectedSite = this; // not sure if this is the right way to do it
        LevelRef.Instance.visitedSiteName = name; // not sure if this is the right way to do it (pretty sure it's not)
        LevelRef.Instance.SetCurrentLevel(levelSO);

        //PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] = 0; //what if it is not there yet, YOU COLLOSAL IDIOT
        PlayerDataMaster.Instance.AddSiteCooldown(levelSO.name, DateTime.Now.Add(levelSO.levelData.waitTime));
        PartyMaster.Instance.currentSquad = readiedSquad;

        levelSO.levelData.isSet = false;
        SceneManager.LoadScene("ArenaSceneGeneric");
    }


    public void StartCooldownCaller()
    {
        StartCoroutine("StartCooldown");
    }
    float timer;
    TimeSpan timerSpan;

    IEnumerator StartCooldown()
    {
        isCooldown = true; //just making sure
        levelSO.levelData.isSet = false;
        //thisButton.interactable = false;
        spriteButton.SetInteractive(false);


        clockAndTimerParent.SetActive(true);
        //timeText.text = "00:" + ((int)((maxCooldown / 60) - (int)timer / 60)).ToString("00") + ":" + ((int)maxCooldown - (int)(timer - (int)timer / 60));

        timeText.text = timerSpan.ToString(@"hh\:mm\:ss");

        while (timerSpan.TotalSeconds >= 0)
        {
            yield return new WaitForSecondsRealtime(clockDelay);
            //timerSpan.Add(new TimeSpan(0,0,clockDelay) += clockDelay;
            timerSpan = timerSpan.Subtract(new TimeSpan(0, 0, clockDelay));
            //int secondsLeft = (int)maxCooldown - (int)timer;  
            //timeText.text = "00:" + (secondsLeft/60).ToString("00") + ":" + (secondsLeft - (secondsLeft/60)*60).ToString("00");
            timeText.text = timerSpan.ToString(@"hh\:mm\:ss");
        }
        clockAndTimerParent.SetActive(false);
        //PlayerDataMaster.Instance.SavedCooldowns[levelSO.name] = maxCooldown; //have a seperate method for AddCooldown that null checks and everything ///06/02 omg this is charming!!!
        PlayerDataMaster.Instance.ClearSiteCooldown(levelSO.name);
        isCooldown = false;
        levelSO.levelData.isSet = true;
        //thisButton.interactable = true;
        spriteButton.SetInteractive(true);

    }

    public void UnSetArrivingSquad() //for cancelExpedition cases
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
        //PartyMaster.Instance.awaySquads.Remove(s);
    }

    public void OnHoverExit()
    {
        //temp until new sprites arrive
        if (_squadPicker.gameObject.activeInHierarchy)
            return;

        //SetHoverColor(false); //this makes it so only on hover over site the site will be coloured, (as opposed to: as long as it's displayer is up
        //myDataDisplay.gameObject.SetActive(false);
        SiteDisplayer.Instance.SetOnOff(false);
        //nothing should have happened, but could be that site-info was open - and then squad arrived... should maybe close site data displayer?
        //if (isCooldown || isWaitingForSquad || isReady)
        //{
        //    //Debug.LogWa("the site is not available to disable display info for");
        //    return;
        //}

    }

    public void OnHoverEnter()
    {
        if (isCooldown || isWaitingForSquad || isReady || _squadPicker.gameObject.activeInHierarchy)
        {
            //Debug.LogError("isCooldown or isWaitingForSquad or ready");
            return;
        }

        SiteDisplayer.Instance.SetOnOff(true);

        //TEMP!
        if(!RevealRing.Instance)
        {
            SiteDisplayer.Instance.SetMe(this, 5);
            return;
        }
        int exposure = RevealRing.Instance.ExposureLevel(siteData);
        SiteDisplayer.Instance.SetMe(this, exposure);

    }

}