using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class practiceDrawBalls : MonoBehaviour
{
    public string instanceName;
    public float timeDelaySecs = 1.0f, startTimeUrnQuestion;
    public Button drawButton, submitButton, urnSubmitButton;
    private List<string> ballDraws;
    public int currentBallDraw = 0, ballDrawsCount;
    private Transform ballContainer,urnAnswerText, colAnswerText, clickButtonText;
    public Transform urnQuestionCanvas, colourQuestionCanvas;
    private Transform blackTemplate, whiteTemplate, purpleTemplate, greenTemplate, urnSliderContainer, colourSliderContainer;
    private GameObject blackBallPrefab, whiteBallPrefab, purpleBallPrefab, greenBallPrefab;

    // Ball layout settings
    public float ballSpaceV = 16.25f;
    public float ballSpaceH = 30f;
    public int maxBallsPerRow = 4;    // Maximum balls per row

    private string jsonFilePath = parameters.inputJsonFilePath;
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
        ballDrawsCount = ballDraws.Count;
        Debug.Log("instanceName: " + instanceName);
    }

    public void OnClick()
    {
        //disable the button
        drawButton.interactable = false;
        
        if (currentBallDraw < ballDrawsCount)
        {
            HideResetBothQuestions();
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

        urnSliderContainer = GameObject.Find("urnSliderContainer")?.transform;
        colourSliderContainer = GameObject.Find("colourSliderContainer")?.transform;

        urnAnswerText = GameObject.Find("urnAnswerText")?.transform;
        colAnswerText = GameObject.Find("colAnswerText")?.transform;
        clickButtonText = GameObject.Find("clickButtonText")?.transform;
    }

    private void HideTwoQuestions()
    {
        SetCanvasGroupVisibility(urnQuestionCanvas, false);
        SetCanvasGroupVisibility(colourQuestionCanvas, false);
    }

    private void HideResetBothQuestions()
    {
        // Deactivate the Correct Answer Text from last round
        urnAnswerText.gameObject.SetActive(false);
        colAnswerText.gameObject.SetActive(false);

        // hide clickButtonText
        clickButtonText.gameObject.SetActive(false);
        // hide both questions
        HideTwoQuestions();

        // reset the sliders to 0
        foreach (Transform child in colourSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                slider.interactable = true;
                slider.value = 0;
                
                // hide the cursor and slider text after submit for the first sequential ball draw
                Transform _cursor = slider.transform.Find("Handle Slide Area");
                Transform _fill = slider.transform.Find("Fill Area");
                Transform _sliderText = slider.transform.Find("sliderText");

                _cursor.gameObject.SetActive(false);
                _fill.gameObject.SetActive(false);
                _sliderText.gameObject.SetActive(false);
            }
        }
        foreach (Transform child in urnSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                slider.interactable = true;
                slider.value = 0;
                
                // hide the cursor and slider text after submit for the first sequential ball draw
                Transform _cursor = slider.transform.Find("Handle Slide Area");
                Transform _fill = slider.transform.Find("Fill Area");
                Transform _sliderText = slider.transform.Find("sliderText");

                _cursor.gameObject.SetActive(false);
                _fill.gameObject.SetActive(false);
                _sliderText.gameObject.SetActive(false);
            }
        }
        // activate the submit button
        submitButton.interactable = true;
        urnSubmitButton.interactable = true;
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
        // Record the start time
        startTimeUrnQuestion = Time.time;
    }

}