using System.Collections;
using System.Linq ;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.IO;

public class GoogleSheetMaster : MonoBehaviour
{
    static string spreadsheetID = "1VUnI_fzIOlRspR7lmM812egyi-kXYP0ltf7qUb7ufQw";
    static string path = "/StreamingAssets/credentials.json";
    static SheetsService sheetsService;

    void SetUpCredentials()
    {
        string fullPath = Application.dataPath + path;

        Stream creds = File.Open(fullPath, FileMode.Open);

        ServiceAccountCredential serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(creds);

        sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = serviceAccountCredential });
    }
    public static GoogleSheetMaster Instance;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Google Sheets Master perfoms start");
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetUpCredentials();
        LogPlayer();
    }
    public void PrintCell()
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, "A1");

        var response = request.Execute();
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            foreach (var row in values)
            {
                Debug.Log(row[0]);
            }

        }
        else
        {
            Debug.Log("No data");
        }
    }
    public List<string> GetRows(string range)
    {
        var request = sheetsService.Spreadsheets.Values.Get(spreadsheetID, range);

        var response = request.Execute();
        var values = response.Values;

        if (values != null && values.Count >= 0)
        {
            //return (List<string>)values;
            List<string> rowsFirstCells = new List<string>();
            foreach (var row in values)
            {
                rowsFirstCells.Add((string)row[0]);
            }
            return rowsFirstCells;
        }
        else
        {
            return null;
        }
    }

    public void LogPlayer()
    {
        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
        var objectList = new List<System.Object>(); //fill object list with things!

        List<string> strings = GetRows("A1:A35"); //currently limited to 35 different players. Disregards any new name after the 35th, but will continue to update any of the first 35
        
        
        if (strings !=null && strings.Count >0 && strings.Contains(PlayerDataMaster.Instance.currentPlayerData.playerName)) 
        {
            //True - the player is already logged:
            int i = strings.IndexOf(PlayerDataMaster.Instance.currentPlayerData.playerName);

            string newRange = "A" + (i+1);
            //objectList = new List<System.Object>() { PlayerDataMaster.Instance.currentPlayerData.playerName, PlayerDataMaster.Instance.currentPlayerData.lostMercs, PlayerDataMaster.Instance.currentPlayerData.numOfavailableMercs, PlayerDataMaster.Instance.currentPlayerData.gold};
            valueRange.Values = new List<IList<object>> { PlayerDataMaster.Instance.GetLog};

            var updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetID, newRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();
        }
        else
        {
            //objectList = new List<System.Object>() { PlayerDataMaster.Instance.currentPlayerData.playerName, PlayerDataMaster.Instance.currentPlayerData.deadMercs, PlayerDataMaster.Instance.currentPlayerData.numOfavailableMercs, PlayerDataMaster.Instance.currentPlayerData.gold };

            valueRange.Values = new List<IList<object>> { PlayerDataMaster.Instance.GetLog };
            //valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = sheetsService.Spreadsheets.Values.Append(valueRange, spreadsheetID, "A2"); //just adds under A
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }
    }
}
