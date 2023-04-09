using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface TurnTaker
{
    int Initiative { get; set; }
    bool TurnDone { get; set; }
    string Name { get; }
    void TakeTurn();

    //TBD Thinking of adding some Special Ability access here... for turn-plate-display purposes

    Sprite PortraitSprite { get; set; }

    TurnInfo TurnInfo { get; set; }
}

