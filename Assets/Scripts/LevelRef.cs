using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRef : MonoBehaviour
{
    public static LevelRef Instance;

    public List<LevelSO> levelSOs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
