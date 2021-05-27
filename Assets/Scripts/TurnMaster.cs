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
        turnPlates.Add(Instantiate(prefabeTurnPlate, turnPlateParent).transform);
        turnPlates[0].localPosition = currenTurnPlateTrans.localPosition;
        //Rect rect = turnPlates[0].GetComponent<RectTransform>().rect;
        turnPlates[0].localScale *= 1.5f;
    

        for (int i = 1; i < turnTakers.Count; i++)
        {
            turnPlates.Add(Instantiate(prefabeTurnPlate, turnPlateParent).transform);

            //Sprites and SA icons are set

            turnPlates[i].localPosition = nextTurnPlateTrans.localPosition + new Vector3((i-1) * turnPlateDistance, 0, 0); //(i-1) because the [1] position in the array is the second plate,
            Image[] imgs = turnPlates[i].GetComponentsInChildren<Image>();
            imgs[0].sprite = turnTakers[i].PortraitSprite;
            //imgs[1].sprite = turnTakers[i].PortraitSprite; //add the SA icon, if applicable
                                                                                                                           //but nextTurnPlateTrans is the second position
                                                                                                                           //(so on i=1, we need to add (1-1)*delta
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
    }
    public void StopTurning()
    {
        StopCoroutine("TurnSequence"); //will need to rearrange lists after. Turn order will be lost
        isGameRunning = false;
        //consider caching currentTurn
    }
    IEnumerator TurnSequence()
    {
        while(isGameRunning)
        {
            if(RefMaster.Instance.mercs.Count == 0 || RefMaster.Instance.enemies.Count == 0)
            {
                isGameRunning = false;
                Debug.Log("Game over!");
                break;
            }
            yield return new WaitForSeconds(turnStartDelay);
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
        

        Time.timeScale = 1; //just in case
        SceneManager.LoadScene(2);
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
        //int delta = currentTurn - currentTurnInDisplayer;

        //if(delta == 0)
        //{
        //    return;
        //}

        //shoud minimize the first/larger portrait?
        //if(turnPlates[0].localScale.x == turnPlates[1].localScale.x)
        turnPlates[0].localScale /= 1.5f;

        Transform t = turnPlates[0];
        turnPlates.Remove(turnPlates[0]);
        turnPlates.Add(t);
        currentTurnInDisplayer++;

        if(currentTurnInDisplayer >= turnPlates.Count) //notice this counts on DISPLAYER PLATES and not TurnTakers
        {
            currentTurnInDisplayer = 0;
        }
        if(currentTurnInDisplayer != currentTurn)
        {
            Debug.LogError("Current display turn and actual currentTurn are not in sync for some ungodly reason. " + 
                currentTurnInDisplayer + " and " + currentTurn);
        }
        
        turnPlates[0].localPosition = currenTurnPlateTrans.localPosition;
        turnPlates[0].localScale *= 1.5f;

        for (int i = 1; i < turnTakers.Count; i++)
        {
            turnPlates[i].localPosition = nextTurnPlateTrans.localPosition +
                new Vector3((i - 1) * turnPlateDistance, 0, 0);

            //(i-1) because the [1] position in the array is the second plate,
            //but nextTurnPlateTrans is the second position
            //(so on i=1, we need to add (1-1)*delta                                                                                                    
        }
    }

    //[ContextMenu("CreateDisplayer")]
    //void CreateTurnDisplayer() // After sort by Initiative
    //{
    //    if(turnDisplayerParent.childCount > 0)
    //    {
    //        int full = turnDisplayerParent.childCount;
    //        for (int i = 0; i < full; i++)
    //        {
    //            Destroy(turnDisplayerParent.GetChild(0).gameObject);
    //        }
    //    }    

    //    foreach(TurnTaker tt in turnTakers)
    //    {
    //        TurnDisplayer td = Instantiate(prefabeTurnPlate, turnDisplayerParent).GetComponent<TurnDisplayer>();
    //        td.nameDisplayer.text = tt.Name;
    //        td.initDisplayer.text = tt.Initiative.ToString();
    //        td.myPawn = (Pawn)tt;
    //        td.pawnImage.sprite = ((Pawn) tt).portraitSprite;
    //        turnDisplayers.Add(td);
    //    }
    //}
    //[ContextMenu("UpdateDisplayer")]
    //void UpdateTurnDisplayer()
    //{
    //    turnDisplayerParent.GetChild(0).SetAsLastSibling();
    //}

    //public void RemoveTurnTaker(Pawn p)
    //{
    //    turnTakers.Remove(p);

    //    if (p.isEnemy)
    //        RefMaster.Instance.enemies.Remove(p);
    //    else
    //        RefMaster.Instance.mercs.Remove(p);

    //    TurnDisplayer tdToDestroy = turnDisplayers.Where(x => x.myPawn.Name == p.Name).SingleOrDefault();
    //    turnDisplayers.Remove(tdToDestroy);
    //    Destroy(tdToDestroy.gameObject);

    //    Debug.Log(name.ToString() + " died as Pawn and removed for relevant list in CharacterMaster");
    //}

   
}
public interface TurnTaker
{
    int Initiative { get; set; }
    bool TurnDone { get; set; }
    string Name { get; set; }
    bool DoDoubleTurn { get; set; }
    bool DoSkipTurn { get; set; }
    void TakeTurn();

    Sprite PortraitSprite { get; set; }
}

