using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextButton : MonoBehaviour
{
    private Button nxtButton;
    private SceneRandomizer sceneRandomizer;

    private void Start()
    {
        DefineVar();
    }

    private void OnClick()
    {
        sceneRandomizer.LoadNextScene();
    }
    
    private void DefineVar()
    {
        nxtButton = GameObject.Find("NextButton")?.GetComponent<Button>();
        nxtButton.onClick.AddListener(OnClick);

        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
    }

}
