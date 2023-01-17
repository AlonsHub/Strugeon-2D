//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TEST_TurnTaker : MonoBehaviour, TurnTaker
//{
//    public int Initiative { get; set; }
//    public bool TurnDone { get ; set ; }

//    [SerializeField]
//    Sprite sprite;

//    public string Name => name;
//    [SerializeField]
//    bool doDoubleTurn;
//    //DEPRECATED?
//    public bool DoDoubleTurn { get => doDoubleTurn; set => doDoubleTurn = value; }
//    //DEPRECATED?
//    public Sprite PortraitSprite { get => sprite; set => sprite = value;}
//    //DEPRECATED?
//    public bool DoSkipTurn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

//    //consider moving this to TurnTaker
//    public TurnInfo turnInfo;

//    Animator anim;
//    public void TakeTurn()
//    {
//        Debug.Log($"{name} took turn");
//        GetComponent<Animator>().SetTrigger("Action");
//    }
//    /// <summary>
//    /// call by animation event
//    /// </summary>
//     public void FinishAnimation()
//    {
//        if (doDoubleTurn)
//        {
//            doDoubleTurn = false; //BAD BUT KEEP FOR NOW! TBD TBF
//            TakeTurn();
//        }
//        else
//        {
//            TurnDone = true;
//        }
        
//    }

//    private void OnMouseDown()
//    {
//        //DoubleTurn_Effect DoubleTurn_Effect = new DoubleTurn_Effect(BeltManipulator.Instance.GetTurnInfoByTaker(this));
//        //DoubleTurn_Effect.ApplyEffect();
//        //HurryTurn_Effect hurryTurn_Effect = new HurryTurn_Effect(BeltManipulator.Instance.GetTurnInfoByTaker(this));
//        //hurryTurn_Effect.ApplyEffect();
//        //BeltManipulator.Instance.RemoveTurnInfo(turnInfo);
//        Destroy(gameObject);
//    }
//    private void OnDestroy()
//    {
//        BeltManipulator.Instance.RemoveTurnInfo(turnInfo);

//    }

//    public TurnInfo TurnInfo()
//    {

//    }

//}
