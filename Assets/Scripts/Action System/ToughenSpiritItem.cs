using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToughenSpiritItem : ActionItem, SA_Item
{
    [SerializeField]
    Sprite toughenSpiritSprite;

    int _currentCooldown;
    [SerializeField]
    int saCooldown;

    private void Start()
    {
        //_currentCooldown = 0; //starts IN cooldown ON! as in gdd
        StartCooldown(); //starts IN cooldown ON! as in gdd

    }

    public override void Action(GameObject tgt)
    {

        StartCooldown();


        
        ///effects should be added to all mercs (as buff effects that kill themselves like bling/charm)

        //GameObject go = Instantiate(healEffect, tgt.transform); 
        //Destroy(go, 2);

        //BattleLogVerticalGroup.Instance.AddEntry(pawn.Name, ActionSymbol.Heal, tgtPawn.Name, healRoll, Color.green);

        //after the effect ->
       //pawn.TurnDone = true;
    }

    public bool SA_Available()
    {
        return !(_currentCooldown > 0);
    }

    public string SA_Description()
    {
        return "Toughen Spirit will grant all of Hadas’s teammates a buff reducing incoming Dmg. Cast as an AoE on all active heroes. At the start of each battle, Toughen Spirit is on cooldown.";
    }

    public string SA_Name()
    {
        return "Toughen Spirit";
        //throw new System.NotImplementedException();
    }

    public Sprite SA_Sprite()
    {
        return toughenSpiritSprite;
    }

    public void StartCooldown()
    {
        _currentCooldown = saCooldown;
    }
}
