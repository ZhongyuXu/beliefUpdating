using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;


public class password : MonoBehaviour
{
    private string correctPwd = "2345";
    private Button passwordButton;
    private TMP_InputField passwordInput;
    public string participantID;
    private Transform warningText;
    private SceneRandomizer sceneRandomizer;


    void Start()
    {
        DefineVar();
    }
    
    void OnClick()
    {
        ValidateParticipantID(passwordInput.text);
    }

    void DefineVar()
    {
        passwordButton = GameObject.Find("passwordButton")?.GetComponent<Button>();
        passwordButton.onClick.AddListener(OnClick);
        
        passwordInput = GameObject.Find("passwordInput")?.GetComponent<TMP_InputField>();

        warningText = GameObject.Find("warningText")?.transform;
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
    }

    private void ValidateParticipantID(string inputPwd)
    {
        if (string.IsNullOrEmpty(inputPwd))
        {
            warningText.GetComponent<Text>().text = "Password cannot be empty.";
            return;
        }

        if (inputPwd != correctPwd)
        {
            warningText.GetComponent<Text>().text = "Incorrect password.";
            return;
        }
        else
        {
            sceneRandomizer.LoadNextScene();
        }
    }

}
