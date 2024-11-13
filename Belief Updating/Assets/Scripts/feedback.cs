using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class feedback : MonoBehaviour
{
    private Transform bonus1;
    private string participantID;

    private pID pIDScript;


    void Start()
    {
        DefineVar();
        ChangeNumber();
    }

    private void DefineVar()
    {
        bonus1 = GameObject.Find("bonus1")?.transform;

        pIDScript = FindObjectOfType<pID>();
        participantID = pIDScript.participantID;
    }

    private void ChangeNumber()
    {
        bonus1.GetComponent<Text>().text = "This is the first line \nThis is the second line";
    }

    // private void LoadReportedJson()
    // {
    //     string jsonFile = File.ReadAllText(parameters.expDataJsonFilePath + "p" + participantID + "BUPractice.json");
    //     if (jsonFile != null)
    //     {
    //         jsonData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(jsonFile);

    //     }
    //     else
    //     {
    //         Debug.LogError("posterior JSON file not found!");
    //     }
    // }    


}
