using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniMercBlock : BasicDisplayer
{
    [SerializeField]
    ExpBarDisplayer expSlider;
    [SerializeField]
    ClassEggPanel classEgg;
    public void SetMeFull(MercSheet ms)
    {
        gameObject.SetActive(true);
        classEgg.SetEgg(ms.mercClass);
        SetMe(new List<string> { ms.characterName.ToString() }, new List<Sprite>() { });
        expSlider.SetBar(ms);
    }
    

}
