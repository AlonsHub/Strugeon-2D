using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ItemInhaleTester : MonoBehaviour
{
    [SerializeField]
    ItemInhaler itemInhaler;

    string output;
    [ContextMenu("Test 10 Items")]
    public void Test10Items()
    {
        output = "";
        StartCoroutine(TestXItems(10));
    }

    IEnumerator TestXItems(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForFixedUpdate(); //as to not overload one update
            output += $"Start Test #{i+1}: ------------------------ \n";
            output += itemInhaler.InhaleSelectedItem();
            output += $"End Test #{i+1} ------------------------ \n\n";
        }

        DirectoryInfo di = Directory.CreateDirectory(Application.dataPath + "/TestLogs");

        StreamWriter sw = File.CreateText($"{Application.dataPath}/TestLogs/{System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt");
        sw.Write(output);
        sw.Close();
    }
    
}
