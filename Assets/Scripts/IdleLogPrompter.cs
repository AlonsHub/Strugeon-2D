using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleLogPrompter : MonoBehaviour
{
    [SerializeField]
    IdleLog idleLog;
    [SerializeField]
    GameObject prefab;
    public void CallLogWithCustomMessage()
    {
        idleLog.RecieveNewMessage(prefab, new List<string>{ "smadi squad", "forest"});
    }
}
