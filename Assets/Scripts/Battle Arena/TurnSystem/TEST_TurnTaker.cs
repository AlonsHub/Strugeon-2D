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

    public void TakeTurn()
    {
        Debug.Log($"{Name} took a turn!");
        TurnDone = true;
    }

   
}
