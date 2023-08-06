using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLogEmpty : MonoBehaviour
{
    public void CheckLogIfEmpty()
    {
        IdleLog.Instance.CloseIfEmptyCheck(1);

        Destroy(gameObject);
    }
}
