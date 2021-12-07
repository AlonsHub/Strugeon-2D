using System.Collections;
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
    static string path = "/Resources/Sheets/credentials.json";
    static SheetsService sheetsService;

    void SetUpCredentials()
    {
        string fullPath = Application.dataPath + path;

        Stream creds = File.Open(fullPath, FileMode.Open);

        ServiceAccountCredential serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(creds);

        sheetsService = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = serviceAccountCredential });
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpCredentials();
        PrintCell();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
