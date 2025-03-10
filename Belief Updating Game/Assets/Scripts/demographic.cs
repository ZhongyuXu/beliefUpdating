using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class demographics : MonoBehaviour
{
    private TMP_InputField pID, payID, fullName, age, strategy;
    private TMP_Dropdown sex, prob, bayes, bayesApplication;
    private bool pIDFilled, payIDFilled, nameFilled, ageFilled, sexFilled, probFilled, bayesFilled, bayesApplicationFilled, strategyFilled;
    private Button nextButton;
    private string participantID;
    private pID pIDScript;


    void Start()
    {
        DefineVar();

        //deactivate next button on start
        nextButton.interactable = false;

        OnValueChange();

        nextButton.onClick.AddListener(SaveData);
    }

    private void DefineVar()
    {
        pID = GameObject.Find("pIDInput").GetComponent<TMP_InputField>();
        payID = GameObject.Find("payIDInput").GetComponent<TMP_InputField>();
        fullName = GameObject.Find("nameInput").GetComponent<TMP_InputField>();
        age = GameObject.Find("ageInput").GetComponent<TMP_InputField>();
        sex = GameObject.Find("sexDropDown").GetComponent<TMP_Dropdown>();
        prob = GameObject.Find("probDropDown").GetComponent<TMP_Dropdown>();
        bayes = GameObject.Find("bayesDropDown").GetComponent<TMP_Dropdown>();
        bayesApplication = GameObject.Find("bayesApplicationDropDown").GetComponent<TMP_Dropdown>();
        strategy = GameObject.Find("strategyInput").GetComponent<TMP_InputField>();

        nextButton = GameObject.Find("NextButton").GetComponent<Button>();

        pIDScript = FindObjectOfType<pID>();
        participantID = pIDScript.participantID;
    }

    private void OnValueChange()
    {
        pID.onValueChanged.AddListener(delegate { CheckEmpty(); });
        payID.onValueChanged.AddListener(delegate { CheckEmpty(); });
        fullName.onValueChanged.AddListener(delegate { CheckEmpty(); });
        age.onValueChanged.AddListener(delegate { CheckEmpty(); });
        sex.onValueChanged.AddListener(delegate { CheckEmpty(); });
        prob.onValueChanged.AddListener(delegate { CheckEmpty(); });
        bayes.onValueChanged.AddListener(delegate { CheckEmpty(); });
        bayesApplication.onValueChanged.AddListener(delegate { CheckEmpty(); });
        strategy.onValueChanged.AddListener(delegate { CheckEmpty(); });
    }

    private void CheckEmpty()
    {
        pIDFilled = CheckEmptyInput(pID);
        payIDFilled = CheckEmptyInput(payID);
        nameFilled = CheckEmptyInput(fullName);
        ageFilled = CheckEmptyInput(age);
        sexFilled = CheckEmptyDropDown(sex);
        probFilled = CheckEmptyDropDown(prob);
        bayesFilled = CheckEmptyDropDown(bayes);
        bayesApplicationFilled = CheckEmptyDropDown(bayesApplication);
        strategyFilled = CheckEmptyInput(strategy);

        nextButton.interactable = pIDFilled && payIDFilled && nameFilled && ageFilled && sexFilled && probFilled && bayesFilled && bayesApplicationFilled && strategyFilled;
    }

    private bool CheckEmptyInput(TMP_InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            return false;
        }
        return true;
    }

    private bool CheckEmptyDropDown(TMP_Dropdown dropDown)
    {
        if (dropDown.value == 0)
        {
            return false;
        }
        return true;
    }

    private void SaveData()
    {
        StringBuilder csvContent = new StringBuilder();
        csvContent.AppendLine("participantID,payID,fullName,age,sex,probSubject,bayesFamaliar,bayesInExp,strategy");

        csvContent.AppendLine($"{pID.text},{payID.text},{fullName.text},{age.text},{sex.options[sex.value].text},{prob.options[prob.value].text},{bayes.options[bayes.value].text},{bayesApplication.options[bayesApplication.value].text},{strategy.text}");

        string directoryPath = parameters.demographicDataCSVFilePath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = directoryPath + "p" + participantID.ToString() + ".csv";
        // int suffix = 1;
        // while (File.Exists(filePath))
        // {
        //     filePath = directoryPath + "p" + participantID.ToString() + "_" + suffix.ToString() + ".csv";
        //     suffix++;
        // }

        File.WriteAllText(filePath, csvContent.ToString());
    }

}