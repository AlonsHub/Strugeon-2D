using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PathCreation;

public class SquadFollower : MonoBehaviour
{
    public Squad squad;
    public TMP_Text timeText;
    float remainingTime;
    PathCreator path;
    [SerializeField]
    float timerRate = .2f;

    PathFollower pathFollower;

    private void Start()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    public void SetMe(Squad s, float t, PathCreator p)
    {
        squad = s;
        remainingTime = t;
        path = p;

        pathFollower.speed = p.path.cumulativeLengthAtEachVertex[p.path.cumulativeLengthAtEachVertex.Length - 1]/t;
        pathFollower.pathCreator = path;

    }

    public void Go()
    {

    }

    IEnumerator RunTimer()
    {
        while (remainingTime >= 0)
        {
            TimeSpan ts = new TimeSpan(0, (int)remainingTime/60, (int)remainingTime);
            timeText.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            yield return new WaitForSeconds(timerRate);
            remainingTime -= timerRate;
        }
    }
}
