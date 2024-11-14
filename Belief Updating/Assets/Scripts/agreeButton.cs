using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class agreeButton : MonoBehaviour
{
    private Button agreeBtn;
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
        agreeBtn = GameObject.Find("AgreeButton")?.GetComponent<Button>();
        agreeBtn.onClick.AddListener(OnClick);

        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
    }

}
