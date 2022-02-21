using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BatllelogEntry : Hoverable
{

    [ColorUsage(true)]
    public Color colRed;
    [ColorUsage(true)]
    public Color colGreen;

    public TMP_Text actingPawnNameDisplayer;
    public Image actionImage;
    public TMP_Text passivePawnNameDisplayer;
    public TMP_Text numberDisplayer;
    
    public string actionDescriptionText; //to be accessed by the displayer
    

    //public override string GetMyData => ReturnMyData();//base.GetMyData;

    //string ReturnMyData()
    //{
    //    return actionDescriptionText;
    //}
    public void Init(string tgtPawn, Sprite psionActionIcon, Color colour)
    {
        //actingPawnNameDisplayer.text = "You cast";
        actingPawnNameDisplayer.text = PlayerDataMaster.Instance.currentPlayerData.playerName;
        actingPawnNameDisplayer.color = colour;

        actionImage.sprite = psionActionIcon;
        //actionImage.SetNativeSize();

        passivePawnNameDisplayer.text = tgtPawn;
        passivePawnNameDisplayer.color = colour;
        numberDisplayer.gameObject.SetActive(false);

        //SetMyData= "You have cast: a" + colour.ToString() +" psion ability on: " + tgtPawn;
    }
    //public void Init(string tgtPawn, Sprite psionActionIcon, string psionicEffect Color colour)
    //{
    //    actingPawnNameDisplayer.text = "";
    //    actionImage.sprite = psionActionIcon;
    //    passivePawnNameDisplayer.text = tgtPawn;
    //    passivePawnNameDisplayer.color = colour;
    //    numberDisplayer.gameObject.SetActive(false);
    //}
    public void Init(string actingName, Sprite actionSprite, ActionSymbol actionSymbol) //unique to Escape!
    {
        actingPawnNameDisplayer.text = actingName;
        actionImage.sprite = actionSprite;
        switch (actionSymbol) //consider using this for all cases! instead of all these overloads TBF
        {
            case ActionSymbol.Attack:
                break;
            case ActionSymbol.Heal:
                break;
            case ActionSymbol.Walk:
                break;
            case ActionSymbol.Censer:
                break;
            case ActionSymbol.Rock:
                break;
            case ActionSymbol.Death:
                break;
            case ActionSymbol.Escape:
        passivePawnNameDisplayer.text = "Escaped!";
                break;
            case ActionSymbol.Summon:
                break;
            case ActionSymbol.Charm:
                break;
            default:
                break;
        }
        //actionImage.SetNativeSize();
        //passivePawnNameDisplayer.text = passiveName;

        numberDisplayer.gameObject.SetActive(false);
    }
    public void Init(string actingName, Sprite actionSprite, string passiveName)
    {
        actingPawnNameDisplayer.text = actingName;
        actionImage.sprite = actionSprite;
        //actionImage.SetNativeSize();
        passivePawnNameDisplayer.text = passiveName;

        numberDisplayer.gameObject.SetActive(false);
    }
    public void Init(string actingName, Sprite actionSprite, string passiveName, int number, Color colour)
    {
        actingPawnNameDisplayer.text = actingName;
        actionImage.sprite = actionSprite;
        //actionImage.SetNativeSize();
        passivePawnNameDisplayer.text = passiveName;

        numberDisplayer.text = number.ToString();

        if (colour == Color.green)
        {
            numberDisplayer.color = colGreen;
            numberDisplayer.text = "+" + numberDisplayer.text;
        }
        else
        {
            numberDisplayer.color = colRed;
            numberDisplayer.text = "-" + numberDisplayer.text;
        }

    }


}
