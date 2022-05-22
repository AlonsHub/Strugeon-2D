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
            Debug.LogError("now level so in " + levelIndex + " " + (LevelEnum)levelIndex);
    }
    public void SetCurrentLevel(LevelSO lso)
    {
        currentLevel = lso;
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    switch (level)
    //    {
    //        case 1:
    //            Instantiate(currentLevel.levelData.levelPrefab);
    //            break;
    //        case 0:
    //            SiteButton[] siteButtons = FindObjectsOfType<SiteButton>();
    //            SiteButton sb = siteButtons.Where(x => x.name == siteName).Single();
    //            sb.StartCooldownCaller();
    //            break;

    //        default:
    //            break;
    //    }
    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex) //more things to be added?
        {
            case 2:
                Instantiate(currentLevel.levelData.levelPrefab);
                break;

            default:
                break;
        }
    }
}
