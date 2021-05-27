using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRock : MonoBehaviour, GridPoser
{
    Vector2Int _gridPos;

    public Vector2Int GetGridPos()
    {
        return _gridPos;
    }

    public string GetName()
    {
        return name;
    }

    public void SetGridPos(Vector2Int newPos)
    {
        _gridPos = newPos;
    }
}
