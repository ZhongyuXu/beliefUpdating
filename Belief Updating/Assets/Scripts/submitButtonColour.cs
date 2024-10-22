using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class submitButtonColour : MonoBehaviour
{
    public Button submitButton, urnSubmitButton, drawButton;
    private Transform sumToOneText, urnSliderContainer, colourSliderContainer, urnQuestionCanvas, colourQuestionCanvas;
    private drawBalls drawBalls;
    private urnTable urnTable;
    private string participantID = "pTest", instanceName;
    private int seqBall;
    private List<Dictionary<string, object>> sliderValuesDict = new List<Dictionary<string, object>>();
    
    public void Start()
    {
        DefineVar();
    }

    public void OnClick()
    {
        bool addToOne = sumToOneCheck();
        if (addToOne)
        {
            LockSliders();
            RecordDataBothQuestions();
            ExportData();
            HideResetBothQuestions();
            UnlockDrawButton();
        }
        else
        {            
            sumToOneText.gameObject.SetActive(true);
        }
    }

    private void DefineVar()
    {
        // slider container is at the same level as the submit button
        urnSliderContainer = GameObject.Find("urnSliderContainer")?.transform;
        colourSliderContainer = transform.parent.Find("colourSliderContainer");
        submitButton.onClick.AddListener(OnClick);

        // sumToOneText is the child of the submit button
        sumToOneText = transform.Find("sumToOneText");
        sumToOneText.gameObject.SetActive(false);  

        drawBalls = FindAnyObjectByType<drawBalls>();
        drawButton = drawBalls.drawButton;
        urnQuestionCanvas = GameObject.Find("Urn Question Canvas")?.transform;
        colourQuestionCanvas = GameObject.Find("Colour Question Canvas")?.transform;

        urnTable = FindObjectOfType<urnTable>();
        instanceName = urnTable.instanceNameMaster;
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
            Debug.Log("Sum: " + sum);
            return false;
        }
        else
        {
            Debug.Log("Sum: " + sum);
            return true;
        }
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
            { "colourPosteriors", new List<float>() }
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
        string filePath = Application.dataPath + "/Resources/"+participantID+instanceName+".json";
        File.WriteAllText(filePath, json);
    }

    private void HideResetBothQuestions()
    {
        // hide both questions
        drawBalls.SetCanvasGroupVisibility(urnQuestionCanvas, false);
        drawBalls.SetCanvasGroupVisibility(colourQuestionCanvas, false);

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
    private void UnlockDrawButton()
    {
        drawButton.interactable = true;
    }
}
