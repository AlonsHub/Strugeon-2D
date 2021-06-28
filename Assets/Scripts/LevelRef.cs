using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelEnum {Forest, Runis, Swamp};
public class LevelRef : MonoBehaviour
{
    public static LevelRef Instance;


    public LevelSO currentLevel;
    public List<LevelSO> levelSOs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentLevel(int levelIndex)
    {
        if (levelSOs[levelIndex])
            currentLevel = levelSOs[levelIndex];
        else
            Debug.LogError("now level so in " + levelIndex + " " + (LevelEnum)levelIndex);
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 1:
                Instantiate(currentLevel.levelData.levelPrefab);
                break;
            default:
                break;
        }
    }
}
