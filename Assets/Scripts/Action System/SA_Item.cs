using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SA_Item 
{
    bool SA_Available();
    void StartCooldown();
    Sprite SA_Sprite();
}
