using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    //[SerializeField]
    //GameObject[] saveDisplayerObjects; //refs to in-scene buttons with SaveFileDisplayer components on them
    List<SaveFileDisplayer> saveFileDisplayers; //also get refs to those objects SaveFileDisplayer 
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Transform parent;

    private void OnEnable()
    {
        if(saveFileDisplayers==null)
        saveFileDisplayers = new List<SaveFileDisplayer>();

        //for (int i = 0; i < saveDisplayerObjects.Length; i++)
        //{
        //    saveFileDisplayers.Add(saveDisplayerObjects[i].GetComponent<SaveFileDisplayer>());
        //}

        if(PlayerDataMaster.Instance != null)
        {
            //Give me all saves please
            int saveCount = 0;
            PlayerData[] loadedData = PlayerDataMaster.Instance.GetPlayerDataFromSaveList();

            if (loadedData == null || loadedData.Length == 0)
            {
                return;
            }

            for (int i = 0; i < loadedData.Length; i++)
            {
                if(i>= saveFileDisplayers.Count)
                {
                    SaveFileDisplayer sfd = Instantiate(prefab, parent).GetComponent<SaveFileDisplayer>();
                    sfd.Init(loadedData[i], gameObject);
                    saveFileDisplayers.Add(sfd);
                }
                else
                {
                    saveFileDisplayers[i].Init(loadedData[i], gameObject);
                }
            }
        }
    }

}
