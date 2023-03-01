using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum LevelEnum {Forest, Runis, Swamp, DarkForest, Lake};
public class LevelRef : MonoBehaviour
{
    public static LevelRef Instance;


    public LevelSO currentLevel;
    public List<LevelSO> levelSOs;

    List<SiteButton> sites; //NOT PUBLIC!


    //public SiteButton siteToCooldown;
    public string visitedSiteName; //WORK AROUND! ----- Fix it by adding cooldown time to SiteButton.SiteCooldowns

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
            Debug.Log("now level so in " + levelIndex + " " + (LevelEnum)levelIndex);
    }
    public void SetCurrentLevel(LevelSO lso)
    {
        currentLevel = lso;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex) //more things to be added?
        {
            case 2:
                GameObject go = Instantiate(currentLevel.levelData.levelPrefab);
                go.transform.Translate(0, 0, -.5f);
                break;

            default:
                break;
        }
    }
}
