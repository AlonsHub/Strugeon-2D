﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnMaster : MonoBehaviour
{
    public static TurnMaster Instance;
    public List<Pawn> turnTakers;
    public int currentTurn;
    public int currentTurnInDisplayer;
    public TurnTaker currentTurnTaker;
    public float turnStartDelay;

    public bool isGameRunning;
    public bool gameHasBeenReset;

    public List<Transform> turnPlates;
    public Transform turnPlateParent;
    public Transform currenTurnPlateTrans;
    public Transform nextTurnPlateTrans;

    
    public GameObject prefabeTurnPlate;
    public float turnPlateDistance;
    
    public List<Bar> bars;

    public float rechargeAmount;

    [SerializeField]
    int turnDisplayerLimit;
    [SerializeField]
    private float endGameCheckRate = 2f;


    //Logged stuff:
    public List<MercName> theDead;
    public List<MercName> theCowardly;

    [SerializeField]
    VictoryWindow victoryWindow;
    [SerializeField]
    DefeatWindow defeatWindow;

    private void Awake()
    {
        Instance = this;
        turnPlates = new List<Transform>();

        theDead = new List<MercName>();
        theCowardly = new List<MercName>();

        gameHasBeenReset = false;
    }


    [ContextMenu("Get Ready")]
    public void GetReady()
    {
        turnTakers = new List<Pawn>();
        // after RefMaster is set
        turnTakers.AddRange(RefMaster.Instance.enemies);
        turnTakers.AddRange(RefMaster.Instance.mercs);

        //roll initiative and Sort
        foreach (TurnTaker tt in turnTakers)
        {
            tt.Initiative = RollDx(20);
        }

        turnTakers.Sort(SortByInitiativeScore);

        //TURN PLATE DISPLAYER!
        GameObject go = Instantiate(prefabeTurnPlate, turnPlateParent);
        turnPlates.Add(go.transform);
        turnPlates[0].localPosition = currenTurnPlateTrans.localPosition;
        //turnPlates[0].localScale *= 1.5f;
        turnTakers[0].myTurnPlate = go;

        TurnDisplayer tp = go.GetComponent<TurnDisplayer>();


        if (tp) //just making sure
        {
            //tp.myPawn = turnTakers[0];
            tp.Init(turnTakers[0]);
            tp.ToggleScale(true);
        }
        else
        {
            Debug.LogError("not turnDisplayer component found on the TurnDisplayer prefab");
        }

        for (int i = 1; i < turnTakers.Count; i++) //start from [1], because [0] is set just above this ^^^^
        {
            GameObject plate = Instantiate(prefabeTurnPlate, turnPlateParent);
            turnPlates.Add(plate.transform);

            //Sprites and SA icons are set

            turnPlates[i].localPosition = nextTurnPlateTrans.localPosition + new Vector3((i - 1) * turnPlateDistance, 0, 0); //(i-1) because the [1] position in the array is the second plate,
                                                                                                                             //Image[] imgs = turnPlates[i].GetComponentsInChildren<Image>();
                                                                                                                             //imgs[0].sprite = turnTakers[i].PortraitSprite;
                                                                                                                             //Image imgs = turnPlates[i].GetComponentInChildren<Image>();
                                                                                                                             //imgs[1].sprite = turnTakers[i].PortraitSprite; //add the SA icon, if applicable
                                                                                                                             //turnPlates[i].GetComponentInChildren<Image>().sprite = turnTakers[i].PortraitSprite;
                                                                                                                             //but nextTurnPlateTrans is the second position
                                                                                                                             //(so on i=1, we need to add (1-1)*delta

            tp = plate.GetComponent<TurnDisplayer>();

            turnTakers[i].myTurnPlate = turnPlates[i].gameObject; //TERRRRIIIBBBLLLEEE
            if (tp) //just making sure
            {
                tp.Init(turnTakers[i]);
                tp.SAIconCheck();
            }
            else
            {
                Debug.LogError("not turnDisplayer component found on the TurnDisplayer prefab");
            }

            turnPlates[i].gameObject.SetActive(i < turnDisplayerLimit); //disables all displayer past turnDisplayerLimit
        }
        //TURN PLATE DISPLAYER!


        StartTurning();
    }
    public void StartTurning() //called by GameMaster ONLY
    {
        currentTurn = 0;
        currentTurnInDisplayer = 0;

        isGameRunning = true;
        StartCoroutine("TurnSequence");
        StartCoroutine("EndGameChecker");
    }
    public void StopTurning()
    {
        if (gameHasBeenReset)
            return; //don't want to stop twice

        StopCoroutine("TurnSequence"); //will need to rearrange lists after. Turn order will be lost
        isGameRunning = false;
        gameHasBeenReset = true;

        foreach (var item in theCowardly)
        {
            theDead.Remove(item); //just in case some escaped mercs also dies somehow
        }

        if (RefMaster.Instance.mercs.Count != 0)
        //if (PartyMaster.Instance.currentMercParty.Count != 0)
        {

            PlayerDataMaster.Instance.currentPlayerData.victories++;

            //Give reward

            Inventory.Instance.AddGold(LevelRef.Instance.currentLevel.levelData.goldReward);
            //DO THIS LIKE A PROGRAMMER PLEASE AND NOT LIKE A PLUMBER! 
            //gold should either be directly linked to between inventory and currentPlayerData - or have an OnValueChanged() for the displayer and player data to subscribe to
            PlayerDataMaster.Instance.currentPlayerData.gold = Inventory.Instance.Gold;
            //PLUMBING

            victoryWindow.gameObject.SetActive(true);
            victoryWindow.SetMe(LevelRef.Instance.currentLevel);

            foreach (var item in theDead)
            {
                PartyMaster.Instance.currentSquad.RemoveMerc(item);
                PlayerDataMaster.Instance.RemoveMercSheet(item);
            }
            foreach (var item in theCowardly)
            {
                PartyMaster.Instance.currentSquad.RemoveMerc(item);
                PlayerDataMaster.Instance.RemoveMercSheet(item);
            }

            if (PartyMaster.Instance.currentSquad.pawns.Count > 0) //returns squad home?
            {
                PartyMaster.Instance.squads.Add(new Squad(PartyMaster.Instance.currentSquad.pawns, PartyMaster.Instance.currentSquad.roomNumber));
            }
            else
            {
                Debug.LogError("Victory,but TurnMaster can't return the squad home, because somehow they're all dead.");
            }



            //Commented out since these mercs should not be available at all

            //foreach (var item in theDead)
            //{
            //    PlayerDataMaster.Instance.currentPlayerData.availableMercs.Remove(item);
            //    Pawn toRemove = PartyMaster.Instance.availableMercs.Where(x => x.mercName == item).FirstOrDefault(); //TEST IF THIS IS EVER NOT DEFAULT!
            //        //PAWNS ARE LIKELY REMOVED ELSEWHERE
            //    PartyMaster.Instance.availableMercs.Remove(toRemove); // is this ever useful?
            //    //PlayerDataMaster.Instance.currentPlayerData.deadMercs++; // set in Victory Window instead, since cowards are dealt there

            //}
            foreach (var item in PartyMaster.Instance.currentSquad.pawns)
            {
                item.GetCharacterSheet.AddExp(LevelRef.Instance.currentLevel.levelData.expReward);
            }
            //put squad back in their room
            PlayerDataMaster.Instance.currentPlayerData.rooms[PartyMaster.Instance.currentSquad.roomNumber].squad = new Squad(PartyMaster.Instance.currentSquad.pawns, PartyMaster.Instance.currentSquad.roomNumber); //werid but it works fine
        }
        else
        {
            PlayerDataMaster.Instance.currentPlayerData.losses++;

            foreach (var item in theCowardly)
            {
                PlayerDataMaster.Instance.currentPlayerData.cowardMercs++;
                PlayerDataMaster.Instance.RemoveMercSheet(item);
            }
            foreach (var item in theDead)
            {
                PlayerDataMaster.Instance.currentPlayerData.deadMercs++;
                PlayerDataMaster.Instance.RemoveMercSheet(item);
            }



            defeatWindow.gameObject.SetActive(true);
            defeatWindow.SetMe(LevelRef.Instance.currentLevel);
            //empty the room:

            PlayerDataMaster.Instance.currentPlayerData.rooms[PartyMaster.Instance.currentSquad.roomNumber] = null;
        }


        PartyMaster.Instance.currentSquad = null;
        //PartyMaster.Instance.currentMercParty.Clear();
        PlayerDataMaster.Instance.GrabAndSaveData();

        Time.timeScale = 1; //just in case
    }

    void LoadLater()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator TurnSequence()
    {
        while (isGameRunning)
        {
            if (RefMaster.Instance.mercs.Count == 0 || RefMaster.Instance.enemies.Count == 0)
            {
                isGameRunning = false;
                Debug.Log("Game over!");
                break;
            }
            yield return new WaitForSeconds(turnStartDelay);

            if (currentTurn >= turnTakers.Count)
            {
                currentTurn = 0;
            }
            currentTurnTaker = turnTakers[currentTurn];

            if (!currentTurnTaker.DoSkipTurn)
            {
                currentTurnTaker.TurnDone = false;
                currentTurnTaker.TakeTurn();
            }
            else
            {
                ((Pawn)currentTurnTaker).RemoveIconByColor("redDeBuff");
                currentTurnTaker.TurnDone = true;
            }
            //MAYBE have an else {currentTurnTaker.TurnDone = true; }


            yield return new WaitUntil(() => currentTurnTaker.TurnDone);

            if (!currentTurnTaker.DoDoubleTurn)
            {
                currentTurn++; //Consider this, maybe don't do it here for doucle and skip turn purposes

                if (currentTurn >= turnTakers.Count)
                {
                    currentTurn = 0;
                    if (bars.Count > 0)
                    {
                        foreach (Bar b in bars)
                        {
                            //b.AddValue(rechargeAmount);
                            b.Regen();
                        }
                    }
                }

                TurnOrderUpdate();
                //    TurnOrderUpdate();
            }
            else
            {
                currentTurnTaker.DoDoubleTurn = false;
                ((Pawn)currentTurnTaker).RemoveIconByColor("blueBuff");
            }


        }

        //PartyMaster.Instance.availableMercs.AddRange(RefMaster.Instance.mercs);
        //PartyMaster.Instance.currentMercParty.Clear();

        //Time.timeScale = 1; //just in case
        //SceneManager.LoadScene(0);
        StopTurning();
    }
    int RollDx(int x)
    {
        return Random.Range(1, x + 1);
    }

    int SortByInitiativeScore(TurnTaker a, TurnTaker b)
    {
        return -a.Initiative.CompareTo(b.Initiative);
    }


  
    void TurnOrderUpdate()
    {
        turnPlates[0].GetComponent<TurnDisplayer>().ToggleScale(false); //shrinks back the Current Turn Portrait

        Transform t = turnPlates[0];
        turnPlates.Remove(turnPlates[0]);
        turnPlates.Add(t);
        

        turnPlates[0].localPosition = currenTurnPlateTrans.localPosition;


        TurnDisplayer td = turnPlates[0].GetComponent<TurnDisplayer>();
        td.ToggleScale(true);

        if (td.hasSA)
        {
            td.SAIconCheck();
        }

        for (int i = 1; i < turnTakers.Count; i++)
        {
            if(i >= turnDisplayerLimit)
            {
                turnPlates[i].gameObject.SetActive(false);
                continue;
            }

            turnPlates[i].gameObject.SetActive(true);

            turnPlates[i].localPosition = nextTurnPlateTrans.localPosition +
                new Vector3((i - 1) * turnPlateDistance, 0, 0);


            td = turnPlates[i].GetComponent<TurnDisplayer>();
            if (td.hasSA)
            {
                td.SAIconCheck();
            }

            //turnPlates[i].gameObject.SetActive(i < turnDisplayerLimit); //disables all displayer past turnDisplayerLimit

            //(i-1) because the [1] position in the array is the second plate,
            //but nextTurnPlateTrans is the second position
            //(so on i=1, we need to add (1-1)*delta                                                                                                    
        }
    }

    

    //void RemovePlate()
    //{

    //}

    //makes sure the game will end once all mercs/enemies die
    IEnumerator EndGameChecker()
    {
        while (isGameRunning)
        {
            yield return new WaitForSeconds(endGameCheckRate);
            if (RefMaster.Instance.mercs.Count == 0 || RefMaster.Instance.enemies.Count == 0)
            {
                isGameRunning = false;
                Debug.Log("Game over!");
                StopTurning();
            }
        }

    }


    public void AddNewTurnTaker(Pawn newPawn) //adding after the current turn taker
    {
        //position self
        newPawn.Init();
        newPawn.tileWalker.FindOwnGridPos();

        GameObject go = Instantiate(prefabeTurnPlate, turnPlateParent);
        go.GetComponent<TurnDisplayer>().Init(newPawn);



        //int newTurnNum = currentTurn + 2 >= turnPlates.Count ? 
        //                (currentTurn + 2) - turnPlates.Count : turnPlates.Count - (currentTurn + 2);

        int newTurnNum = currentTurn - 1 > 0 ? currentTurn - 1 : turnPlates.Count-1;

        turnPlates.Insert(newTurnNum, go.transform);
        turnTakers.Add(newPawn);


    }

   
}
public interface TurnTaker
{
    int Initiative { get; set; }
    bool TurnDone { get; set; }
    string Name { get; }
    bool DoDoubleTurn { get; set; }
    bool DoSkipTurn { get; set; }
    void TakeTurn();

    Sprite PortraitSprite { get; set; }
}

