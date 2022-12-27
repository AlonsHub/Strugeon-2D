using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TurnTaker : MonoBehaviour, TurnTaker
{
    public int Initiative { get; set; }
    public bool TurnDone { get ; set ; }

    public string Name => name;

    //DEPRECATED
    public bool DoDoubleTurn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool DoSkipTurn { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Sprite PortraitSprite { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //DEPRECATED

    Animator anim;
    public void TakeTurn()
    {
        GetComponent<Animator>().SetTrigger("Action");
    }
    /// <summary>
    /// call by animation event
    /// </summary>
     public void FinishAnimation()
    {
        //for now, just
        TurnDone = true;
        //bu here we can handle double turn
    }

    private void OnMouseDown()
    {
        SkipTurn_Effect skipTurn_Effect = new SkipTurn_Effect(BeltManipulator.Instance.GetTurnInfoByTaker(this));
        skipTurn_Effect.ApplyEffect();
    }
}
