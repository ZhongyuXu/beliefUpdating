using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class pID : MonoBehaviour
{
    private Button submitButtonID;
    private TMP_InputField inputFieldID;
    public string participantID;

    private SceneRandomizer sceneRandomizer;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DefineVar();
    }
    
    void OnClick()
    {
        participantID = inputFieldID.text;
        Debug.Log("Participant ID: " + participantID);
        sceneRandomizer.LoadNextScene();
    }

    void DefineVar()
    {
        submitButtonID = GameObject.Find("idButton")?.GetComponent<Button>();
        submitButtonID.onClick.AddListener(OnClick);
        
        inputFieldID = GameObject.Find("idInput")?.GetComponent<TMP_InputField>();
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
    }

}
