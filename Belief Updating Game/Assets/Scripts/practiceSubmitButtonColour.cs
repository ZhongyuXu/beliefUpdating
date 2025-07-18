using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.SceneManagement;

public class practiceSubmitButtonColour : MonoBehaviour
{
    public Button submitButton, urnSubmitButton, drawButton;
    public float delay,responseTimeColourQuestion;

    private Transform sumToOneText,urnSliderContainer, colourSliderContainer, urnAnswerText, colAnswerText, clickButtonText;
    private practiceDrawBalls drawBalls;
    private urnTable urnTable;
    private SceneRandomizer sceneRandomizer;
    private practiceSubmitButtonUrn submitButtonUrn;
    private pID pIDScript;

    private string participantID, instanceName;
    private int seqBall, numUrn, numCol;
    private string posteriorJsonFilePath = parameters.posteriorsJsonFilePath;
    public Dictionary<string, Dictionary<string, float>> jsonData;
    private List<string> alphabets, cols, urn_answers, col_answers;
    private List<urnTable.UrnInfo> urnEntryList;
    private List<Dictionary<string, object>> sliderValuesDict = new List<Dictionary<string, object>>();


    public void Start()
    {
        DefineVar();
        LoadPosteriorJson();
    }

    public void OnClick()
    {
        bool addToOne = sumToOneCheck();
        if (addToOne)
        {
            LockSliders();
            responseTimeColourQuestion = Time.time - submitButtonUrn.startTimeColourQuestion;
            RecordDataBothQuestions();
            ExportData();

            ShowCorrectAnswer();

            ShowClickButtonText();

            // jump to next scene (instance) if no more balls to draw
            if (drawBalls.currentBallDraw == drawBalls.ballDrawsCount)
            {
                StartCoroutine(ShowNextSceneWithDelay());
            }

        }
        else
        {            
            sumToOneText.gameObject.SetActive(true);
        }
    }

    private void DefineVar()
    {
        pIDScript = FindObjectOfType<pID>();
        participantID = pIDScript.participantID;

        // slider container is at the same level as the submit button
        urnSliderContainer = GameObject.Find("urnSliderContainer")?.transform;
        colourSliderContainer = transform.parent.Find("colourSliderContainer");
        submitButton.onClick.AddListener(OnClick);

        submitButtonUrn = FindAnyObjectByType<practiceSubmitButtonUrn>();

        // sumToOneText is the child of the submit button
        sumToOneText = transform.Find("sumToOneText");
        sumToOneText.gameObject.SetActive(false);  

        drawBalls = FindAnyObjectByType<practiceDrawBalls>();
        drawButton = drawBalls.drawButton;


        urnTable = FindObjectOfType<urnTable>();
        instanceName = urnTable.instanceNameMaster;

        sceneRandomizer = FindObjectOfType<SceneRandomizer>();

        urnEntryList = urnTable.urnEntryList;

        numUrn = urnEntryList.Count -1;
        numCol = urnEntryList[0].composition.Count;

        urnAnswerText = GameObject.Find("urnAnswerText")?.transform;
        colAnswerText = GameObject.Find("colAnswerText")?.transform;
        clickButtonText = GameObject.Find("clickButtonText")?.transform;
    }
    
    private void LockSliders()
    {
        foreach (Transform child in colourSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                slider.interactable = false;
            }
        }
        sumToOneText.gameObject.SetActive(false);
        // inactivate the submit button
        submitButton.interactable = false;
    }
    
    private bool sumToOneCheck()
    {
        float sum = 0;
        foreach (Transform child in colourSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                sum += slider.value;
            }
        }
        if (sum != 100)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    

    private void UnlockDrawButton()
    {
        drawButton.interactable = true;
    }

    private void LoadPosteriorJson()
        {
            string jsonFile = File.ReadAllText(posteriorJsonFilePath);
            if (jsonFile != null)
            {
                jsonData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(jsonFile);

            }
            else
            {
                Debug.LogError("posterior JSON file not found!");
            }
        }    

    private void ShowCorrectAnswer()
    {
        seqBall = drawBalls.currentBallDraw; 

        urnAnswerText.gameObject.SetActive(true);
        alphabets = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        urn_answers = new List<string>();
        for (int i = 0; i < numUrn; i++)
        {
            urn_answers.Add( "Urn " + alphabets[i] + ": " +
                (jsonData[instanceName]["posterior_u" + (i+1).ToString() + "_draw" + seqBall.ToString()]*100).ToString() + "%");
        }

        urnAnswerText.GetComponent<Text>().text = "The correct answer is: " + string.Join(", ", urn_answers);


        colAnswerText.gameObject.SetActive(true);
        cols = new List<string> { "Black", "White", "Purple", "Green"};
        col_answers = new List<string>();
        for (int i = 0; i < numCol; i++)
        {
            col_answers.Add( cols[i] + ": " +
                (jsonData[instanceName]["posterior_col" + (i+1).ToString() + "_draw" + seqBall.ToString()]*100).ToString() + "%");
        }

        colAnswerText.GetComponent<Text>().text = "The correct answer is: " + string.Join(", ", col_answers);
    }

    private void ShowClickButtonText()
    {
        StartCoroutine(ShowClickButtonTextWithDelay());
    }

    private IEnumerator ShowClickButtonTextWithDelay()
    {
        yield return new WaitForSeconds(delay);
        clickButtonText.gameObject.SetActive(true);
        UnlockDrawButton();
    }

        
    private void RecordDataBothQuestions()
    {
        seqBall = drawBalls.currentBallDraw;

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "participantID", participantID },
            { "instanceName", instanceName },
            { "seqBall", seqBall},
            { "urnPosteriors", new List<float>() },
            { "colourPosteriors", new List<float>() },
            { "responseTimeUrn", submitButtonUrn.responseTimeUrnQuestion },
            { "responseTimeColour", responseTimeColourQuestion }
        };

        bool firstSliderSkipped = false;
        foreach (Transform child in urnSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                if (!firstSliderSkipped)
                {
                    firstSliderSkipped = true;
                    continue;
                }
                
                ((List<float>)data["urnPosteriors"]).Add(slider.value);
            }
        }

        bool firstSliderSkippedColour = false;
        foreach (Transform child in colourSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                if (!firstSliderSkippedColour)
                {
                    firstSliderSkippedColour = true;
                    continue;
                }

                ((List<float>)data["colourPosteriors"]).Add(slider.value);
            }
        }

        sliderValuesDict.Add(data);
    }

    private void ExportData()
    {
        string json = JsonConvert.SerializeObject(sliderValuesDict, Formatting.Indented);

        string directoryPath = parameters.expDataJsonFilePath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = directoryPath + "p" + participantID + instanceName + ".json";
        
        // int suffix = 1;
        // while (File.Exists(filePath))
        // {
        //     filePath = directoryPath + "p" + participantID + instanceName + "_" + suffix + ".json";
        //     suffix++;
        // }
        File.WriteAllText(filePath, json);
    }

    private IEnumerator ShowNextSceneWithDelay()
    {
        yield return new WaitForSeconds(delay);
        sceneRandomizer.LoadNextScene();
    }

}
