using System.Collections;
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

    public List<TurnDisplayer> turnPlates;
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
    private float endGameCheckRate = 1f;


    //Logged stuff:
    public List<MercName> theDead;
    public List<MercName> theCowardly;

    [SerializeField]
    VictoryWindow victoryWindow;
    [SerializeField]
    DefeatWindow defeatWindow;


    public System.Action OnTurnOrderRestart;
    private void Awake()
    {
        Instance = this;
        turnPlates = new List<TurnDisplayer>();

        theDead = new List<MercName>();
        theCowardly = new List<MercName>();

        gameHasBeenReset = false;
    }


    [ContextMenu("Get Ready")]
    public void GetReady()
    {
        turnTakers = new List<Pawn>();
        // after RefMaster is set
        turnTakers.AddRange(RefMaster.Instance.enemyInstances);
        turnTakers.AddRange(RefMaster.Instance.mercs);

        //roll initiative and Sort
        foreach (TurnTaker tt in turnTakers)
        {
            tt.Initiative = RollDx(20);
        }

        turnTakers.Sort(SortByInitiativeScore);

        //TURN PLATE DISPLAYER!
        GameObject go = Instantiate(prefabeTurnPlate, turnPlateParent);
        TurnDisplayer tp = go.GetComponent<TurnDisplayer>();
        turnPlates.Add(tp);
        turnPlates[0].transform.localPosition = currenTurnPlateTrans.localPosition; //the first and only at the moment
        //turnPlates[0].localScale *= 1.5f;
        turnTakers[0].myTurnPlate = tp;

        //TurnDisplayer tp = go.GetComponent<TurnDisplayer>();


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
            tp = plate.GetComponent<TurnDisplayer>();
            turnPlates.Add(tp);

            //Sprites and SA icons are set

            turnPlates[i].transform.localPosition = nextTurnPlateTrans.localPosition + new Vector3((i - 1) * turnPlateDistance, 0, 0); //(i-1) because the [1] position in the array is the second plate,
                                                                                                                             //Image[] imgs = turnPlates[i].GetComponentsInChildren<Image>();
                                                                                                                             //imgs[0].sprite = turnTakers[i].PortraitSprite;
                                                                                                                             //Image imgs = turnPlates[i].GetComponentInChildren<Image>();
                                                                                                                             //imgs[1].sprite = turnTakers[i].PortraitSprite; //add the SA icon, if applicable
                                                                                                                             //turnPlates[i].GetComponentInChildren<Image>().sprite = turnTakers[i].PortraitSprite;
                                                                                                                             //but nextTurnPlateTrans is the second position
                                                                                                                             //(so on i=1, we need to add (1-1)*delta


            //turnTakers[i].myTurnPlate = turnPlates[i]; 
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

        //Time.timeScale = 1; //just in case
        
        StopCoroutine("TurnSequence"); //will need to rearrange lists after. Turn order will be lost
        isGameRunning = false;
        gameHasBeenReset = true;

        MouseBehaviour.Instance.ShutDown();

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
            PlayerDataMaster.Instance.currentPlayerData.gold = Inventory.Instance.Gold; //WHAT ?!>!??!?!
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
                PlayerDataMaster.Instance.currentPlayerData.availableMercs.Add(item);
                MercPrefabs.Instance.EnumToPawnPrefab(item).mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
                //PlayerDataMaster.Instance.RemoveMercSheet(item); //sheet remains!
            }

            if (PartyMaster.Instance.currentSquad.pawns.Count > 0) //returns squad home?
            {
                if(!PartyMaster.Instance.squads.Contains(PartyMaster.Instance.currentSquad)) //to prevent simpleSites (which do NOT remove squads) from adding duplicates 20/03/22 TBF AF (PartyMaster needs changing)
                PartyMaster.Instance.squads.Add(new Squad(PartyMaster.Instance.currentSquad.pawns, PartyMaster.Instance.currentSquad.roomNumber));
            }
            else
            {
                Debug.LogError("Victory,but TurnMaster can't return the squad home, because somehow they're all dead.");
            }



            #region 1) Divided Exp Reward
            ////Total and Shared Exp:
            ////this way a total sum of exp is divided by surviving mercs

            int expPerMerc = LevelRef.Instance.currentLevel.levelData.expReward / PartyMaster.Instance.currentSquad.pawns.Count;

            foreach (var item in PartyMaster.Instance.currentSquad.pawns)
            {
                item.mercSheetInPlayerData.AddExp(expPerMerc);
            }
            #endregion

            #region 2) Constant Value Exp Reward

            //foreach (var item in PartyMaster.Instance.currentSquad.pawns)
            //{
            //    item.mercSheetInPlayerData.AddExp(LevelRef.Instance.currentLevel.levelData.expReward);
            //}
            #endregion

            #region Single MagicItem Reward

            Inventory.Instance.AddMagicItem(LevelRef.Instance.currentLevel.levelData.magicItem);

            #endregion
            //put squad back in their room
            PlayerDataMaster.Instance.currentPlayerData.rooms[PartyMaster.Instance.currentSquad.roomNumber].squad = new Squad(PartyMaster.Instance.currentSquad.pawns, PartyMaster.Instance.currentSquad.roomNumber); //werid but it works fine



        }
        else
        {
            PlayerDataMaster.Instance.currentPlayerData.losses++;

            foreach (var item in theCowardly)
            {
                //PlayerDataMaster.Instance.currentPlayerData.cowardMercs++;
                PlayerDataMaster.Instance.currentPlayerData.availableMercs.Add(item);
                MercPrefabs.Instance.EnumToPawnPrefab(item).mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
                //PlayerDataMaster.Instance.RemoveMercSheet(item);
            }
            foreach (var item in theDead)
            {
                //PlayerDataMaster.Instance.currentPlayerData.deadMercs++;
                PlayerDataMaster.Instance.RemoveMercSheet(item);
            }



            defeatWindow.gameObject.SetActive(true);
            defeatWindow.SetMe(LevelRef.Instance.currentLevel);
            //empty the room:

            PlayerDataMaster.Instance.currentPlayerData.rooms[PartyMaster.Instance.currentSquad.roomNumber].ClearRoom(); //FIXED to ClearRoom() from = null
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
            if (RefMaster.Instance.mercs.Count == 0 || RefMaster.Instance.enemyInstances.Count == 0)
            {
                isGameRunning = false;
                Debug.Log("Game over!");
                break;
            }
            yield return new WaitForSeconds(turnStartDelay);

            if (currentTurn >= turnTakers.Count) //I don't think this ever happens
            {
                currentTurn = 0;
            }
            currentTurnTaker = turnTakers[currentTurn];

            TurnOrderUpdate(1); //int 1 to engage overload

            if (!currentTurnTaker.DoSkipTurn)
            {
                currentTurnTaker.TurnDone = false;
                currentTurnTaker.TakeTurn();
            }
            else
            {
                ((Pawn)currentTurnTaker).RemoveIconByName("redDeBuff");
                currentTurnTaker.DoSkipTurn = false;
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

                    OnTurnOrderRestart?.Invoke();

                    //if (bars.Count > 0)
                    //{
                    //    foreach (Bar b in bars)
                    //    {
                    //        //b.AddValue(rechargeAmount);
                    //        b.Regen();
                    //    }
                    //}
                }

                //TurnOrderUpdate(1);
                //    TurnOrderUpdate();
            }
            else
            {
                currentTurnTaker.DoDoubleTurn = false;
                ((Pawn)currentTurnTaker).RemoveIconByName("blueBuff");
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


    public void RemoveTurnPlate(TurnDisplayer turnPlate)
    {
        turnPlates.Remove(turnPlate);

        //order check
        
    }
    public void RemoveTurnTaker(TurnTaker tt)
    {
        turnTakers.Remove(tt as Pawn);
        turnPlates.Remove((tt as Pawn).myTurnPlate);
        Destroy((tt as Pawn).myTurnPlate.gameObject);

        //SetTurnOrder();
    }
    void SetTurnOrder() //doesnt advance the order, just sets it - also safe checks
    {
        turnTakers[0].myTurnPlate.transform.localPosition = currenTurnPlateTrans.localPosition;
        turnTakers[0].myTurnPlate.ToggleScale(true);

        if (turnTakers[0].myTurnPlate.hasSA)
        {
            turnTakers[0].myTurnPlate.SAIconCheck();
        }

        for (int i = 1; i < turnDisplayerLimit; i++)
        {
            turnTakers[i].myTurnPlate.gameObject.SetActive(true);
            turnTakers[i].myTurnPlate.ToggleScale(false);
            turnTakers[i].myTurnPlate.transform.localPosition = nextTurnPlateTrans.localPosition +
                                                     new Vector3((i - 1) * turnPlateDistance, 0, 0);
            if(turnTakers[i].myTurnPlate.hasSA)
            turnTakers[i].myTurnPlate.SAIconCheck();
        }
        for (int i = turnDisplayerLimit; i < turnTakers.Count; i++)
        {
            //turnTakers[i].myTurnPlate.ToggleScale(false);
            turnTakers[i].myTurnPlate.gameObject.SetActive(false);
        }
    }
    void TurnOrderUpdate()
    {
        //turnPlates[0].GetComponent<TurnDisplayer>().ToggleScale(false); //shrinks back the Current Turn Portrait
        turnPlates[0].ToggleScale(false); //shrinks back the Current Turn Portrait

        TurnDisplayer t = turnPlates[0];
        turnPlates.Remove(turnPlates[0]);
        turnPlates.Add(t);
        

        turnPlates[0].transform.localPosition = currenTurnPlateTrans.localPosition;


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

            turnPlates[i].transform.localPosition = nextTurnPlateTrans.localPosition +
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
    void TurnOrderUpdate(int previousTurn) //not really needed, just keeping it as over load
    {
        //turnPlates[0].GetComponent<TurnDisplayer>().ToggleScale(false); //shrinks back the Current Turn Portrait
        //turnPlates[previousTurn].ToggleScale(false); //shrinks back the Current Turn Portrait
        //turnPlates[previousTurn].gameObject.SetActive(false); //will be SetActive(true) if needed


        turnPlates[currentTurn].transform.localPosition = currenTurnPlateTrans.localPosition;
        turnPlates[currentTurn].ToggleScale(true);

        if (turnPlates[currentTurn].hasSA)
        {
            turnPlates[currentTurn].SAIconCheck();
        }

        int count = 0;
        for (int i = currentTurn+1; i < turnPlates.Count(); i++)
        {
            count++;
            if(count >= turnDisplayerLimit)
            {
                turnPlates[i].gameObject.SetActive(false);
                continue;
            }
            turnPlates[i].gameObject.SetActive(true);
            turnPlates[i].ToggleScale(false);

            turnPlates[i].transform.localPosition = nextTurnPlateTrans.localPosition +
                new Vector3((count-1) * turnPlateDistance, 0, 0);

            if(turnPlates[i].hasSA)
            {
                turnPlates[i].SAIconCheck();
            }
        } //this counts to the end of the row

        //now count FORWARD from 0 to current turn-1
        for (int i = 0; i < currentTurn; i++)
        {
            count++;
            if (count >= turnDisplayerLimit)
            {
                turnPlates[i].gameObject.SetActive(false);
                continue;
            }
            turnPlates[i].gameObject.SetActive(true);
            turnPlates[i].ToggleScale(false);

            turnPlates[i].transform.localPosition = nextTurnPlateTrans.localPosition +
                new Vector3((count-1) * turnPlateDistance, 0, 0);

            if (turnPlates[i].hasSA)
            {
                turnPlates[i].SAIconCheck();
            }
        }


    }


    //makes sure the game will end once all mercs/enemies die
    IEnumerator EndGameChecker()
    {
        while (isGameRunning)
        {
            yield return new WaitForSeconds(endGameCheckRate);
            if (RefMaster.Instance.mercs.Count == 0 || RefMaster.Instance.enemyInstances.Count == 0)
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

        GameObject turnPlateGo = Instantiate(prefabeTurnPlate, turnPlateParent);
        TurnDisplayer td =
        turnPlateGo.GetComponent<TurnDisplayer>();
        td.Init(newPawn);

        turnPlateGo.SetActive(false);

        //int newTurnNum = currentTurn + 2 >= turnPlates.Count ? 
        //                (currentTurn + 2) - turnPlates.Count : turnPlates.Count - (currentTurn + 2);

        //int newTurnNum = (currentTurn - 1) > 0 ? currentTurn - 1 : turnPlates.Count-1; //if (true) - should add to currentTurn++, or it would bump the summoner 1 turn later
        //giving them the next turn!!!

        //int newTurnNum;
        //if(cu)
        ///why not just add to the end - even if it is the next turn?


        //turnPlates.Insert(newTurnNum, td);
        //turnTakers.Insert(newTurnNum, newPawn);
        turnPlates.Add(td );
        turnTakers.Add(newPawn);

        TurnOrderUpdate(1);
        //turnTakers.Add(newPawn);


    }

    public void Run() //Abandon match - used by forefit button in UI
    {
        foreach (var item in RefMaster.Instance.mercs)
        {
            //if not contanis in either coawrdly nor dead, add to cowardly
            theCowardly.Add(item.mercName); //check for doubles?

        }
        RefMaster.Instance.mercs.Clear();
        StartCoroutine(nameof(StopTurning));
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

