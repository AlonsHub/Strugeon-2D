using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] saveDisplayerObjects; //refs to in-scene buttons with SaveFileDisplayer components on them
    SaveFileDisplayer[] saveFileDisplayers; //also get refs to those objects SaveFileDisplayer 

    private void OnEnable()
    {
        saveFileDisplayers = new SaveFileDisplayer[saveDisplayerObjects.Length];
        for (int i = 0; i < saveDisplayerObjects.Length; i++)
        {
            saveFileDisplayers[i] = saveDisplayerObjects[i].GetComponent<SaveFileDisplayer>();
        }

        if(PlayerDataMaster.Instance != null)
        {
            //Give me all saves please
        int saveCount = 0;
            PlayerData[] loadedData = PlayerDataMaster.Instance.GetPlayerDataFromSaveList();

            foreach (var item in loadedData)
            {
                saveFileDisplayers[saveCount].Init(item);
            }
        }
    }

}
