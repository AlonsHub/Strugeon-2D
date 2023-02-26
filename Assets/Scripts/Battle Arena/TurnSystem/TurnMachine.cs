using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnMachine : MonoBehaviour
{
    public static TurnMachine Instance;
    //[SerializeField]
    //TurnBelt turnBelt;

    [SerializeField]
    VictoryWindow victoryWindow;
    [SerializeField]
    DefeatWindow defeatWindow;

    [SerializeField]
    BeltManipulator beltManipulator;
    //[SerializeField]
    //DisplayBelt displayBelt;
    [SerializeField]
    float startOfTurnDelay;
    /// <summary>
    /// Keeps the turnsequence while loop going.
    /// </summary>
    bool isActiveBattle= false;

    public System.Action OnNextTurn;
    /// <summary>
    /// Basically, what happens on the StartPin's turn
    /// </summary>
    public System.Action OnStartNewRound;
    private int initialSquadSize;

    PsionSpectrumProfile psionProfile => PlayerDataMaster.Instance.currentPlayerData.psionSpectrum;


    private void Awake() //this does destroy on load and should only ever be one, in the arena
    {
        Instance = this;
        OnStartNewRound += psionProfile.PerformRegenAll;
    }
    private void OnDisable()
    {
        Instance = null;
        OnStartNewRound -= psionProfile.PerformRegenAll;
    }

    public TurnTaker GetCurrentTurnTaker => beltManipulator.GetCurrentTurnTaker();

    public void GetReady()
    {
        List<TurnTaker> tts = new List<TurnTaker>();

        initialSquadSize = RefMaster.Instance.mercs.Count;

        tts.AddRange(RefMaster.Instance.enemyInstances);
        tts.AddRange(RefMaster.Instance.mercs);

        SetMachine(tts);

        Invoke(nameof(StartBattle), 2f);
    }

    public void SetMachine(List<TurnTaker> allTakers)
    {
        beltManipulator.InitManipulator(allTakers);

    }
    /// <summary>
    /// For now, the Machine recieves the first order of buisness. Recieving All TurnTakers, passing it on to the BeltManipulator to init the belt.
    /// The machine will them wait patiently till "StartBattle" is called(?)
    /// </summary>
    /// 
    public void StartBattle()
    {
        //TBA a priliminary sequence just before StartBattle for setup such as this

        //displayBelt.Init(this); //TBF do this earlier


        //CALL OnBattleBegin!
        isActiveBattle = true;
        StartCoroutine(nameof(TurnSequence));
    }
    IEnumerator TurnSequence()
    {
        TurnInfo currentTurnInfo;
        while (isActiveBattle)
        {
            currentTurnInfo = beltManipulator.NextTurn();//Skipping the first processing of the Start-Pin, allowing us to differentiate events such as: "On Round Restart" and "On Begin Battle"

            if (currentTurnInfo.isStartPin)
            {
                Debug.Log("Round Restarted!");
                OnStartNewRound?.Invoke();
                //call events for StartPin! (On Round Restart) //should call for the on start and end events for start pin? they could be useful
                continue;
            }

            OnNextTurn?.Invoke();

            if(RefMaster.Instance.enemyInstances.Count == 0)
            {
                Debug.Log("WIN!");
                Win();
                //Return mercs home?

                StopTurnSequence();
                yield break;
            }
            if (RefMaster.Instance.mercs.Count == 0)
            {
                Debug.Log("Lose!");
                Lose();
                //Clear mercs room?

                StopTurnSequence();
                yield break;
            }

            SkipTurn_Effect se;
            if (currentTurnInfo.GetEffectOfType(out se))
            {
                //currentTurnInfo.OnTurnSkip?.Invoke();
                currentTurnInfo.RemoveEffect(se);

                Debug.Log($"{currentTurnInfo.GetTurnTaker.Name}'s turn was skipped!");
                //TBD figure out if turn-skips also count against Cooldowns, but I suppose that can be done by sub/unsubbing from on turn start events? OR simply not calling them... ergh
                // ^^^ Solved -> Add a method to TurnTaker, which performs a "skipped-turn's" logic... if needed, a RemoveHold/CountDurationOfEffect method will sub to an event like "OnSkippedTurn" if relevant
                // 
                // Call OnSkippedTurn action here
                continue;
            }

            yield return new WaitForSeconds(startOfTurnDelay);

            currentTurnInfo.IsTurnDone = false;
            currentTurnInfo.TakeTurn();

            yield return new WaitUntil(() => currentTurnInfo.IsTurnDone);
            currentTurnInfo.OnTurnEnd?.Invoke();
        }

    }
    /// <summary>
    /// Checks if win or lose states apply. If either, returns TRUE, and calls Win() or Lose(), AND StopTurnSequence().
    /// </summary>
    /// <returns></returns>
    bool WinLoseCheck()
    {
        if (RefMaster.Instance.enemyInstances.Count != 0 && RefMaster.Instance.mercs.Count != 0)
        {
            return false;
        }
        if (RefMaster.Instance.enemyInstances.Count == 0)
        {
            Debug.Log("WIN!");
            Win();
        }
        else
        {
            Debug.Log("Lose!");
            Lose();
        }
        //if (RefMaster.Instance.mercs.Count == 0)

        StopTurnSequence();
        return true;
    }
    public void StopTurnSequence()
    {
        StopCoroutine(nameof(TurnSequence));
        
        MouseBehaviour.Instance.ShutDown();
        PartyMaster.Instance.currentSquad = null;

        RefMaster.Instance.ClearDeadAndCowards();

        Time.timeScale = 1; //just in case
    }

    public List<TurnInfo> GetTurnInfos()
    {
        return beltManipulator.GetTurnInfos();
    }

    public void InsertTurnInfo(TurnInfo ti, int index)
    {
        beltManipulator.InsertTurnInfo(ti, index);
    }

    public void InsertTurnTaker(TurnTaker tt, int index)
    {
        beltManipulator.InsertTurnTaker(tt, index);
    }
    public void InsertTurnTakerAsNext(TurnTaker tt)
    {
        beltManipulator.InsertTurnTakerAsNext(tt);
    }
    public void RemoveTurnInfoByTaker(TurnTaker tt)
    {
        //beltManipulator.RemoveTurnInfo(beltManipulator.GetTurnInfoByTaker(tt));
        beltManipulator.RemoveTurnInfo(tt.TurnInfo);
    }
    public void RemoveTurnInfo(TurnInfo ti)
    {
        beltManipulator.RemoveTurnInfo(ti);
    }
    public void RemoveALLInfosForTaker(TurnTaker tt)
    {
        beltManipulator.RemoveALLInfoByTaker(tt);
    }

    private void Win()
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

        foreach (var item in RefMaster.Instance.GetTheDead)
        {
            PartyMaster.Instance.currentSquad.RemoveMerc(item);
            PlayerDataMaster.Instance.RemoveMercSheet(item);
        }
        foreach (var item in RefMaster.Instance.GetTheCowardly)
        {
            PartyMaster.Instance.currentSquad.RemoveMerc(item);
            PlayerDataMaster.Instance.currentPlayerData.availableMercNames.Add(item);
            MercPrefabs.Instance.EnumToPawnPrefab(item).mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
            //PlayerDataMaster.Instance.RemoveMercSheet(item); //sheet remains!
        }

        if (PartyMaster.Instance.currentSquad.pawns.Count > 0) //returns squad home?
        {
            if (!PartyMaster.Instance.squads.Contains(PartyMaster.Instance.currentSquad)) //to prevent simpleSites (which do NOT remove squads) from adding duplicates 20/03/22 TBF AF (PartyMaster needs changing)
                PartyMaster.Instance.squads.Add(new Squad(PartyMaster.Instance.currentSquad.pawns, PartyMaster.Instance.currentSquad.roomNumber));
        }
        else
        {
            Debug.LogError("Victory,but TurnMaster can't return the squad home, because somehow they're all dead.");
        }



        #region 1) Divided Exp Reward
        ////Total and Shared Exp:
        ////this way a total sum of exp is divided by surviving mercs

        //int expPerMerc = LevelRef.Instance.currentLevel.levelData.expReward / PartyMaster.Instance.currentSquad.pawns.Count;
        int expPerMerc = LevelRef.Instance.currentLevel.levelData.expReward / initialSquadSize;

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
    private void Lose()
    {
        PlayerDataMaster.Instance.currentPlayerData.losses++;

        foreach (var item in RefMaster.Instance.GetTheCowardly)
        {
            //PlayerDataMaster.Instance.currentPlayerData.cowardMercs++;
            PlayerDataMaster.Instance.currentPlayerData.availableMercNames.Add(item);
            MercPrefabs.Instance.EnumToPawnPrefab(item).mercSheetInPlayerData.SetToState(MercAssignment.Available, -1);
            //PlayerDataMaster.Instance.RemoveMercSheet(item);
        }
        foreach (var item in RefMaster.Instance.GetTheDead)
        {
            //PlayerDataMaster.Instance.currentPlayerData.deadMercs++;
            PlayerDataMaster.Instance.RemoveMercSheet(item);
        }



        //Considered putting StopTurnSequence(); here and in Win, but honestly that just seems silly. They're both called in WinLoseCheck() - that should call StopTurnSequence()

        defeatWindow.gameObject.SetActive(true);
        defeatWindow.SetMe(LevelRef.Instance.currentLevel);
        //empty the room:

        PlayerDataMaster.Instance.currentPlayerData.rooms[PartyMaster.Instance.currentSquad.roomNumber].ClearRoom(); //FIXED to ClearRoom() from = null
    }

    public void Run() //Abandon match - used by forefit button in UI
    {
        foreach (var item in RefMaster.Instance.mercs)
        {
            //if not contanis in either coawrdly nor dead, add to cowardly
            RefMaster.Instance.GetTheCowardly.Add(item.mercName); //check for doubles?

        }
        RefMaster.Instance.mercs.Clear();
        //StopTurnSequence();
        WinLoseCheck();
    }
}
