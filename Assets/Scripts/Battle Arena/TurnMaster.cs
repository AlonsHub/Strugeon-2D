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

    public List<Transform> turnPlates;
    public Transform turnPlateParent;
    public Transform currenTurnPlateTrans;
    public Transform nextTurnPlateTrans;

    //public TurnDisplayer turnDisplayer;
    //public Transform turnDisplayerParent;
    public GameObject prefabeTurnPlate;
    public float turnPlateDistance;
    //List<TurnDisplayer> turnDisplayers;

    //public TurnDisplayer turnDisplayer;
    public List<Bar> bars;

    public float rechargeAmount;

    [SerializeField]
    int turnDisplayerLimit;
    [SerializeField]
    private float endGameCheckRate = 2f;

    private void Awake()
    {
        Instance = this;
        turnPlates = new List<Transform>();
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
        turnPlates[0].localScale *= 1.5f;
        turnTakers[0].myTurnPlate = go;

        TurnDisplayer tp = go.GetComponent<TurnDisplayer>();


        if (tp) //just making sure
        {
            //tp.myPawn = turnTakers[0];
            tp.Init(turnTakers[0]);
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

            turnTakers[i].myTurnPlate = turnPlates[i].gameObject;
            if (tp) //just making sure
            {
                tp.Init(turnTakers[i]);
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
        StopCoroutine("TurnSequence"); //will need to rearrange lists after. Turn order will be lost
        isGameRunning = false;

        if (RefMaster.Instance.mercs.Count != 0)
        {
            //Give reward

            Inventory.Instance.Gold += LevelRef.Instance.currentLevel.levelData.goldReward;
            foreach (var item in RefMaster.Instance.mercs)
            {
                PartyMaster.Instance.availableMercs.Add(MercPrefabs.Instance.EnumToPrefab(item.mercName).GetComponent<Pawn>());
            }
            
        }

        //PartyMaster.Instance.currentMercParty.Clear();

        Time.timeScale = 1; //just in case
        SceneManager.LoadScene(0);
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
                            b.AddValue(rechargeAmount);
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
        turnPlates[0].localScale /= 1.5f;

        Transform t = turnPlates[0];
        turnPlates.Remove(turnPlates[0]);
        turnPlates.Add(t);
        //turnPlates.(t);
        currentTurnInDisplayer++;

        if (currentTurnInDisplayer >= turnPlates.Count) //notice this counts on DISPLAYER PLATES and not TurnTakers
        {
            currentTurnInDisplayer = 0;
        }
        if (currentTurnInDisplayer != currentTurn)
        {
            Debug.LogWarning("Current display turn and actual currentTurn are not in sync for some ungodly reason. " +
                currentTurnInDisplayer + " and " + currentTurn);
        }

        turnPlates[0].localPosition = currenTurnPlateTrans.localPosition;
        turnPlates[0].localScale *= 1.5f;

        TurnDisplayer td = turnPlates[0].GetComponent<TurnDisplayer>();

        if (td.hasSA)
        {
            td.SAIconCheck();
        }

        for (int i = 1; i < turnTakers.Count; i++)
        {
            turnPlates[i].localPosition = nextTurnPlateTrans.localPosition +
                new Vector3((i - 1) * turnPlateDistance, 0, 0);


            td = turnPlates[i].GetComponent<TurnDisplayer>();
            if (td.hasSA)
            {
                td.SAIconCheck();
            }

            turnPlates[i].gameObject.SetActive(i < turnDisplayerLimit); //disables all displayer past turnDisplayerLimit

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

