using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercFilter_CrewBuilder : MonoBehaviour
{
    [SerializeField]
    MercClass filterTo;
    [SerializeField]
    SquadBuilder2 squadBuilder;

    public void LimitSquadBuilder()
    {
        squadBuilder.SetClassFilterOnRoster(filterTo);
    }

}
