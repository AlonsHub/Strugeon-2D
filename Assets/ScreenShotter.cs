using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShotter : MonoBehaviour
{
    [SerializeField, Tooltip("Blank is the game's Root folder. Format folder path as 'Assets/Screenshots/Test1' - FIRST AND LAST '/' are implied (code will check for this anyways, just to make sure). If no folder by this name exists in this path, one will be created.")]
    string folderName; 
    [SerializeField, Tooltip("Constant text at the begining of each screenshot file name. E.g. 'RingersScreenshot_' \n This field can remain blank.")]
    string fileNamePrefix;
    [SerializeField, Tooltip("Multiplies the resolution of the exporte screenshots. Default: 4")]
    int resolutionUpScale = 4;

    [SerializeField, Tooltip("Default: F12")]
    KeyCode captureKey = KeyCode.F12;


    string _fullPathName;
    bool _doScreenShot;

    string _previousFolderName; //used to check if user changed the Screenshot directory
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        UpdateDirectory();

        //_fullPathName = 
    }

    private void UpdateDirectory()
    {
        CleanFirstAndLastSlashesFromFolderName(); //safety measure

        _fullPathName = $"{Application.dataPath}/{folderName}/";

        //DirectoryInfo directoryInfo = Directory.CreateDirectory(_fullPathName); //if it folder exists, this will only return the path to that folder (wont create a new one)
        Directory.CreateDirectory(_fullPathName); //if it folder exists, this will only return the path to that folder (wont create a new one)
        _previousFolderName = folderName;
    }

    private void CleanFirstAndLastSlashesFromFolderName()
    {
        int length = folderName.Length;
        int start = 0;
        if (folderName[0] == '/')
        {
            start = 1;
            length--;
        }
        if (folderName[folderName.Length - 1] == '/')
        {
            length--;
        }
        folderName = folderName.Substring(start, length);
    }

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += TryDoScreenShot;
        
    }
    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= TryDoScreenShot;
        
    }


    void Update()
    {
        if(Input.GetKeyDown(captureKey))
        {
            _doScreenShot = true;
        }
        
    }
    void TryDoScreenShot(ScriptableRenderContext scriptableRenderContext, Camera cam)
    {
        if(_doScreenShot)
        {
            if(folderName != _previousFolderName)
            {
                UpdateDirectory();
            }

            ScreenCapture.CaptureScreenshot($"{_fullPathName}{fileNamePrefix}_{System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.png", resolutionUpScale);
            _doScreenShot = false;
        }
    }
}
