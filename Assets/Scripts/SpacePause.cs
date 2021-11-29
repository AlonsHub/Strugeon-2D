using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePause : MonoBehaviour
{
    bool isPause = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isPause)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;

            isPause = !isPause;
        }
    }
}