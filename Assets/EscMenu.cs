using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    [SerializeField]
    GameObject menuObject;
    [SerializeField]
    RandomItemGetter randomItemGetter;

    void Update()
    {
        if(EscToX.Current)
        { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Input.GetKey(KeyCode.Space))

                randomItemGetter.OnClick();
            else
                menuObject.SetActive(!menuObject.activeSelf);
        }
    }
}
