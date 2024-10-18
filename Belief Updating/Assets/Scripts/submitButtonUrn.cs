using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submitButtonUrn : MonoBehaviour
{
    public Button submitButton;
    private Transform sumToOneText, urnSliderContainer;
    public void Start()
    {
        DefineVar();
        
    }

    public void OnClick()
    {
        Debug.Log("Submit Button Clicked");
        bool addToOne = sumToOneCheck();
        if (addToOne)
        {
            LockSliders();
        }
        else
        {}
    }

    private void DefineVar()
    {
        // slider container is at the same level as the submit button
        urnSliderContainer = transform.parent.Find("urnSliderContainer");
        submitButton.onClick.AddListener(OnClick);

        // sumToOneText is the child of the submit button
        sumToOneText = transform.Find("sumToOneText");
        sumToOneText.gameObject.SetActive(false);  
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
        sumToOneText.gameObject.SetActive(false);
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
            sumToOneText.gameObject.SetActive(true);
            return false;
        }
        else
        {
            Debug.Log("Sum: " + sum);
            return true;
        }
    }
}
