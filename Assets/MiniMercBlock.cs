using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniMercBlock : BasicDisplayer
{
    [SerializeField]
    ExpBarDisplayer expSlider;
    public void SetMeFull(MercSheet ms)
    {
        SetMe(new List<string> { ms.characterName.ToString(), ms.mercClass.ToString() }, new List<Sprite>() { });
        expSlider.SetBar(ms);
    }
    

}
