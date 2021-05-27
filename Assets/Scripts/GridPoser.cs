using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GridPoser 
{
    Vector2Int GetGridPos();
    void SetGridPos(Vector2Int newPos);
    string GetName();
}
