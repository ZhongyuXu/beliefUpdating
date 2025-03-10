using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;


public class pID : MonoBehaviour
{
    private Button submitButtonID;
    private TMP_InputField inputFieldID;
    public string participantID;
    private Transform dupIDText;
    private List<string> participantIDs = new List<string>();
    private string filePath = parameters.expDataJsonFilePath + "participantIDs.txt";

    private SceneRandomizer sceneRandomizer;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DefineVar();
        ReadExistingIDs();
    }
    
    void OnClick()
    {
        participantID = inputFieldID.text;
        ValidateParticipantID(participantID);
    }

    void DefineVar()
    {
        submitButtonID = GameObject.Find("idButton")?.GetComponent<Button>();
        submitButtonID.onClick.AddListener(OnClick);
        
        inputFieldID = GameObject.Find("idInput")?.GetComponent<TMP_InputField>();
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();

        dupIDText = GameObject.Find("dupIDText")?.transform;

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.Create(filePath).Dispose();
        }
    }

    private void ValidateParticipantID(string inputID)
    {
        if (string.IsNullOrEmpty(inputID))
        {
            dupIDText.GetComponent<Text>().text = "Participant ID cannot be empty.";
            return;
        }

        if (participantIDs.Contains(inputID))
        {
            // ID is a duplicate
            dupIDText.GetComponent<Text>().text  = $"Participant ID \"{inputID}\" already exists.";
            inputFieldID.text = ""; // Clear the input field
        }
        else
        {
            // ID is unique, add to the collection
            File.AppendAllText(filePath, inputID+"\n");
            Debug.Log("Participant ID: " + participantID);
            ExportInstanceSequence(inputID);
            sceneRandomizer.LoadNextScene();
        }
    }

    private void ReadExistingIDs()
    {
        string[] idsFromFile = File.ReadAllLines(filePath);
        foreach (string id in idsFromFile)
            {
                participantIDs.Add(id.Trim());
            }
    }

    private void ExportInstanceSequence(string id)
    {
        string seqfilePath = parameters.expDataJsonFilePath + "p" + id + "_instanceSequence.txt";
        if (!File.Exists(seqfilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(seqfilePath));
            File.Create(seqfilePath).Dispose();
        }
        File.WriteAllLines(seqfilePath, sceneRandomizer.sceneList);
    }

}
