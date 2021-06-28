using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLauncher : MonoBehaviour
{
    public static ArenaLauncher Instance;

    private void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    } //Mono-Singleton

    public void LoadArena(LevelSO levelSO)
    {

    }
}
