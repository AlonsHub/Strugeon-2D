using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleLogMessage : BasicDisplayer
{
    public IdleLogOrder myOrder; //not always used - set remotely when relevant

    public override void BasicDisplayersButton(bool doDestroy)
    {
        IdleLog.backLog.Remove(myOrder);

        base.BasicDisplayersButton(doDestroy);
        IdleLog.Instance.CloseIfEmptyCheck(1); //only 1 message is to be destroyed and will still count against logParent.ChildCount
    }
}
