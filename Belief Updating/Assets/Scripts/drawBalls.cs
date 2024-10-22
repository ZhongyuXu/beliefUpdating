using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class drawBalls : MonoBehaviour
{
    public string instanceName;
    public float timeDelaySecs = 1.0f;
    public Button drawButton;
    private List<string> ballDraws;
    public int currentBallDraw = 0;
    private Transform ballContainer;
    public Transform urnQuestionCanvas, colourQuestionCanvas;
    private Transform blackTemplate, whiteTemplate, purpleTemplate, greenTemplate;
    private GameObject blackBallPrefab, whiteBallPrefab, purpleBallPrefab, greenBallPrefab;

    // Ball layout settings
    public float ballSpaceV = 16.25f;
    public float ballSpaceH = 16.25f;
    public int maxBallsPerRow = 4;    // Maximum balls per row

    private string jsonFilePath = parameters.jsonFilePath;
    private urnTable urnTable;

    [System.Serializable]

    public class UrnInfo
    {
        public string urnName { get; set; }
        public string prior { get; set; }
        public List<string> composition { get; set; }
        public int balls { get; set; }
    }

    public class BU
    {
        public List<UrnInfo> urnInfo { get; set; }
        public string chosenUrn { get; set; }
        public List<string> ballDraws { get; set; }
    }

    public void Start()
    {
        DefineVar();
        HideTwoQuestions();
        // Load the ball draws from the JSON file
        ballDraws = LoadBallDrawsFromJson();
        Debug.Log("instanceName: " + instanceName);
    }

    public void OnClick()
    {
        //disable the button
        drawButton.interactable = false;
        // figure out how many ball draws in this instance
        int ballDrawsCount = ballDraws.Count;
        
        if (currentBallDraw < ballDrawsCount)
        {
            string ballDraw = ballDraws[currentBallDraw];
            CreateBallDraws(ballDraw, ballContainer, currentBallDraw);
            currentBallDraw++;
            ShowUrnQuestionWithDelay();
        }

    }

    public List<string> LoadBallDrawsFromJson()
    {
        // Read the JSON file content
        string jsonString = File.ReadAllText(jsonFilePath);

        // Deserialize the JSON content into a list of dictionaries
        List<Dictionary<string, BU>> rootList = JsonConvert.DeserializeObject<List<Dictionary<string, BU>>>(jsonString);

        // Find the BU instance with the specified instance name
        foreach (var root in rootList)
        {
            if (root.ContainsKey(instanceName))
            {
                // Return the ball draws list for the specified instance
                return root[instanceName].ballDraws;
            }
        }

        // If the instance name is not found, return an empty list
        return new List<string>();
    }

    private void DefineVar()
    {
        urnTable = FindObjectOfType<urnTable>();
        instanceName = urnTable.instanceNameMaster;

        ballContainer = transform.parent.Find("ballDrawArea").Find("ballContainer");
        blackTemplate = ballContainer.Find("blackTemplate");
        whiteTemplate = ballContainer.Find("whiteTemplate");
        purpleTemplate = ballContainer.Find("purpleTemplate");
        greenTemplate = ballContainer.Find("greenTemplate");

        blackTemplate.gameObject.SetActive(false);
        whiteTemplate.gameObject.SetActive(false);
        purpleTemplate.gameObject.SetActive(false);
        greenTemplate.gameObject.SetActive(false);
     
        blackBallPrefab = blackTemplate.gameObject;
        whiteBallPrefab = whiteTemplate.gameObject;
        purpleBallPrefab = purpleTemplate.gameObject;
        greenBallPrefab = greenTemplate.gameObject;

        urnQuestionCanvas = GameObject.Find("Urn Question Canvas")?.transform;
        colourQuestionCanvas = GameObject.Find("Colour Question Canvas")?.transform;

        drawButton.onClick.AddListener(OnClick);
    }

    private void HideTwoQuestions()
    {
        SetCanvasGroupVisibility(urnQuestionCanvas, false);
        SetCanvasGroupVisibility(colourQuestionCanvas, false);
    }
    public void SetCanvasGroupVisibility(Transform canvas, bool isVisible)
    {
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
    // This function generates balls based on the composition list from the JSON
    private void CreateBallDraws(string balldraw, Transform container, int ballDrawSequence)
    {
        // Get the correct ball prefab based on the color code
        GameObject ballPrefab = GetBallPrefabFromColorCode(balldraw);
        Transform template = GetTemplateFromColorCode(balldraw);

        if (ballPrefab == null)
        {
            Debug.LogError("Invalid color code: " + balldraw);
            return;
        }

        // Instantiate 1 balls for this color
        int row = ballDrawSequence / maxBallsPerRow;
        int column = ballDrawSequence % maxBallsPerRow;
        Transform entryTransform = Instantiate(template, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        float forwardZPosition = 0f;

        entryRectTransform.anchoredPosition = new Vector3(
            ballSpaceH * column, 
            ballSpaceV * row, 
            forwardZPosition);

        entryTransform.gameObject.SetActive(true);
        
    }

    // Function to get the correct ball prefab based on color code
    private GameObject GetBallPrefabFromColorCode(string colorCode)
    {
        GameObject ball = colorCode switch
        {
            "B" => blackBallPrefab,
            "W" => whiteBallPrefab,
            "P" => purpleBallPrefab,
            "G" => greenBallPrefab,
            _ => null,  // Add more colors if needed
        };
        return ball;
    }

    private Transform GetTemplateFromColorCode(string colorCode)
    {
        return colorCode switch
        {
            "B" => blackTemplate,
            "W" => whiteTemplate,
            "P" => purpleTemplate,
            "G" => greenTemplate,
            _ => null,  // Add more colors if needed
        };

    }
    private void ShowUrnQuestionWithDelay()
    {
        StartCoroutine(ShowUrnQuestionAfterDelay());
    }

    private IEnumerator ShowUrnQuestionAfterDelay()
    {
        yield return new WaitForSeconds(timeDelaySecs);
        SetCanvasGroupVisibility(urnQuestionCanvas, true);
    }

}