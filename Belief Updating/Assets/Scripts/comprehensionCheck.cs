using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class comprehensionCheck : MonoBehaviour
{
    private TMP_Dropdown Q1DropDown, Q2DropDown, Q3DropDown, Q4DropDown, Q5DropDown;
    private Transform Q1Prompt, Q2Prompt, Q3Prompt, Q4Prompt, Q5Prompt;
    private Button nextButton;

    void Start()
    {
        DefineVar();

        //deactivate next button on start
        nextButton.interactable = false;

        CheckAnswerOnValueChanged();
    }

    private void DefineVar()
    {
        Q1DropDown = GameObject.Find("Q1DropDown").GetComponent<TMP_Dropdown>();
        Q2DropDown = GameObject.Find("Q2DropDown").GetComponent<TMP_Dropdown>();
        Q3DropDown = GameObject.Find("Q3DropDown").GetComponent<TMP_Dropdown>();
        Q4DropDown = GameObject.Find("Q4DropDown").GetComponent<TMP_Dropdown>();
        Q5DropDown = GameObject.Find("Q5DropDown").GetComponent<TMP_Dropdown>();

        Q1Prompt = GameObject.Find("Q1Prompt")?.transform;
        Q2Prompt = GameObject.Find("Q2Prompt")?.transform;
        Q3Prompt = GameObject.Find("Q3Prompt")?.transform;
        Q4Prompt = GameObject.Find("Q4Prompt")?.transform;
        Q5Prompt = GameObject.Find("Q5Prompt")?.transform;

        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
    }

    private void CheckAnswerOnValueChanged()
    {
        Q1DropDown.onValueChanged.AddListener(delegate { DropdownValueChanged(Q1DropDown, Q1Prompt); });
        Q2DropDown.onValueChanged.AddListener(delegate { DropdownValueChanged(Q2DropDown, Q2Prompt); });
        Q3DropDown.onValueChanged.AddListener(delegate { DropdownValueChanged(Q3DropDown, Q3Prompt); });
        Q4DropDown.onValueChanged.AddListener(delegate { DropdownValueChanged(Q4DropDown, Q4Prompt); });
        Q5DropDown.onValueChanged.AddListener(delegate { DropdownValueChanged(Q5DropDown, Q5Prompt); });
    }

    private void DropdownValueChanged(TMP_Dropdown change, Transform prompt)
    {
        if (change.value == 0)
        {
            prompt.GetComponent<Text>().text = " ";
        }
        else if (change.value == 1)
        {
            prompt.GetComponent<Text>().text = "That's correct";
        }
        else if (change.value == 2)
        {
            prompt.GetComponent<Text>().text = "That's wrong, try again";
        }

        if (Q1DropDown.value == 1 && Q2DropDown.value == 1 && Q3DropDown.value == 1 && Q4DropDown.value == 1 && Q5DropDown.value == 1)
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }
    }
}
