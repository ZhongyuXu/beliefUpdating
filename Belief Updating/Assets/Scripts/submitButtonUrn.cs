using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submitButtonUrn : MonoBehaviour
{
    public Button submitButton;
    private Transform sumToOneText, urnSliderContainer,colourQuestionCanvas;
    private drawBalls drawBalls;
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
            ShowColourQuestion();
        }
        else
        {
            sumToOneText.gameObject.SetActive(true);
        }
    }

    private void DefineVar()
    {
        // slider container is at the same level as the submit button
        urnSliderContainer = transform.parent.Find("urnSliderContainer");
        submitButton.onClick.AddListener(OnClick);

        // sumToOneText is the child of the submit button
        sumToOneText = transform.Find("sumToOneText");
        sumToOneText.gameObject.SetActive(false); 

        drawBalls = FindAnyObjectByType<drawBalls>();
        colourQuestionCanvas = GameObject.Find("Colour Question Canvas")?.transform;
    }
    private void LockSliders()
    {
        foreach (Transform child in urnSliderContainer)
        {
            if (child.TryGetComponent<Slider>(out Slider slider))
            {
                slider.interactable = false;
            }
        }
        // turn off the warning text
        sumToOneText.gameObject.SetActive(false);
        // inactivate the submit button
        submitButton.interactable = false;
    }
    private bool sumToOneCheck()
    {
        float sum = 0;
        foreach (Transform child in urnSliderContainer)
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
    private void ShowColourQuestion()
    {
        drawBalls.SetCanvasGroupVisibility(colourQuestionCanvas, true);
    }
}
