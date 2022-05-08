﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMercDisplayer : BasicDisplayer
{
    [SerializeField]
    ExpBarDisplayer expBarDisplayer;
    MercPoolDisplayer mercPoolDisplayer;
    MercSheet sheet;
    public bool SetMeFull(MercSheet mercSheet, MercPoolDisplayer poolDisplayer)
    {
        mercPoolDisplayer = poolDisplayer;
        sheet = mercSheet;

        Pawn p = mercSheet.MyPawnPrefabRef<Pawn>();
        List<string> textsPerTextBox = new List<string> { mercSheet.characterName.ToString(), mercSheet.currentAssignment.ToString(), mercSheet._maxHp.ToString(), $"{mercSheet._minDamage} - {mercSheet._maxDamage}" };// = new List<string>();
        List<Sprite> spritesPerImage = new List<Sprite> {p.PortraitSprite, p.SASprite};


        expBarDisplayer.SetBar(mercSheet);

        return base.SetMe(textsPerTextBox, spritesPerImage);
    }

    public void OnMyClick()
    {
        //display merc to the MercGearDisplayer
        mercPoolDisplayer.ShowMercOnGearDisplayer(sheet);
    }
}
