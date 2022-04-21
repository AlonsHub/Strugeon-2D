using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SA_Item 
{
    string SA_Name();
    bool SA_Available();
    void StartCooldown();
    Sprite SA_Sprite();

    void SetToLevel(int level);

    string SA_Description(); //probably useless
}
