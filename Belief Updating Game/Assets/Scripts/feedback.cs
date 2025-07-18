using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class feedback : MonoBehaviour
{
    public float multiplier = 1f;
    private Transform bonus1, bonus2, bonus3, bonus4, finalPay, bonus1Calculation, bonus2Calculation, bonus3Calculation, bonus4Calculation;
    private Button nextButton;
    private string participantID;
    private List<Dictionary<string, object>> reportedData;
    private Dictionary<string, Dictionary<string, float>> jsonData;

    private pID pIDScript;
    private string posteriorJsonFilePath = parameters.posteriorsJsonFilePath;


    void Start()
    {
        DefineVar();
        //deactivate next button on start
        nextButton.interactable = false;
        // activate next button after 10 seconds
        StartCoroutine(ActivateNextButton());

        LoadReportedJson();
        LoadPosteriorJson();
        ChangeText(multiplier);
    }

    private void DefineVar()
    {
        bonus1 = GameObject.Find("bonus1")?.transform;
        bonus2 = GameObject.Find("bonus2")?.transform;
        bonus3 = GameObject.Find("bonus3")?.transform;
        bonus4 = GameObject.Find("bonus4")?.transform;
        finalPay = GameObject.Find("finalPay")?.transform;
        bonus1Calculation = GameObject.Find("bonus1Calculation")?.transform;
        bonus2Calculation = GameObject.Find("bonus2Calculation")?.transform;
        bonus3Calculation = GameObject.Find("bonus3Calculation")?.transform;
        bonus4Calculation = GameObject.Find("bonus4Calculation")?.transform;

        pIDScript = FindObjectOfType<pID>();
        // need to uncomment this line later
        participantID = pIDScript.participantID;

        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
    }

    private IEnumerator ActivateNextButton()
    {
        yield return new WaitForSeconds(10);
        nextButton.interactable = true;
    }

    private void ChangeText(float multiplier)
    {
        // Bonus 1 Urn A after the third ball draw
        float bonus1_reported = JsonConvert.DeserializeObject<List<float>>(
            reportedData[2]["urnPosteriors"].ToString())[0];

        float bonus1_correct = (jsonData["BUPractice"]["posterior_u1_draw3"]*100);

        float bonus1_reward = Mathf.Max(0, 10 - multiplier * Mathf.Abs(bonus1_reported - bonus1_correct));
        bonus1_reward = Mathf.Round(bonus1_reward * 100f) / 100f;

        bonus1.GetComponent<Text>().text = 
        "You reported " + bonus1_reported.ToString() + 
        "% chance that the secretly selected urn being Urn A after the third ball draw.\nThe correct answer is " +
        bonus1_correct.ToString() + "%.";

        bonus1Calculation.GetComponent<Text>().text = "Your first bonus is max($0, $10 -" + multiplier.ToString() + "*|" +  bonus1_reported.ToString() + " - " + bonus1_correct.ToString() + "|) = $" + 
        bonus1_reward.ToString() + ".";

        // Bonus 2 Colour Black after the second ball draw. Black is the first colour in the list, white is the second
        float bonus2_reported = JsonConvert.DeserializeObject<List<float>>(
            reportedData[1]["colourPosteriors"].ToString())[0];

        float bonus2_correct = (jsonData["BUPractice"]["posterior_col1_draw2"]*100);

        float bonus2_reward = Mathf.Max(0, 10 - multiplier * Mathf.Abs(bonus2_reported - bonus2_correct));
        bonus2_reward = Mathf.Round(bonus2_reward * 100f) / 100f;

        bonus2.GetComponent<Text>().text = 
        "You reported " + bonus2_reported.ToString() + 
        "% chance that next ball draw colour being black after the second ball draw.\nThe correct answer is " +
        bonus2_correct.ToString() + "%.";

        bonus2Calculation.GetComponent<Text>().text = "Your second bonus is max($0, $10 -" + multiplier.ToString() + "*|" +  bonus2_reported.ToString() + " - " + bonus2_correct.ToString() + "|) = $" + 
        bonus2_reward.ToString() + ".";

        // Bonus 3 Colour White after the first ball draw. Black is the first colour in the list, white is the second
        float bonus3_reported = JsonConvert.DeserializeObject<List<float>>(
            reportedData[0]["colourPosteriors"].ToString())[1];

        float bonus3_correct = (jsonData["BUPractice"]["posterior_col2_draw1"]*100);

        float bonus3_reward = Mathf.Max(0, 10 - multiplier * Mathf.Abs(bonus3_reported - bonus3_correct));
        bonus3_reward = Mathf.Round(bonus3_reward * 100f) / 100f;

        bonus3.GetComponent<Text>().text = 
        "You reported " + bonus3_reported.ToString() + 
        "% chance that next ball draw colour being white after the first ball draw.\nThe correct answer is " +
        bonus3_correct.ToString() + "%.";

        bonus3Calculation.GetComponent<Text>().text = "Your third bonus is max($0, $10 -" + multiplier.ToString() + "*|" +  bonus3_reported.ToString() + " - " + bonus3_correct.ToString() + "|) = $" + 
        bonus3_reward.ToString() + ".";

        // Bonus 4 Urn B after the second ball draw. 
        float bonus4_reported = JsonConvert.DeserializeObject<List<float>>(
            reportedData[1]["urnPosteriors"].ToString())[1];

        float bonus4_correct = (jsonData["BUPractice"]["posterior_u2_draw2"]*100);

        float bonus4_reward = Mathf.Max(0, 10 - multiplier * Mathf.Abs(bonus4_reported - bonus4_correct));
        bonus4_reward = Mathf.Round(bonus4_reward * 100f) / 100f;

        bonus4.GetComponent<Text>().text = 
        "You reported " + bonus4_reported.ToString() + 
        "% chance that the secretly selected urn being Urn B after the second ball draw.\nThe correct answer is " +
        bonus4_correct.ToString() + "%.";

        bonus4Calculation.GetComponent<Text>().text = "Your fourth bonus is max($0, $10 -" + multiplier.ToString() + "*|" +  bonus4_reported.ToString() + " - " + bonus4_correct.ToString() + "|) = $" + 
        bonus4_reward.ToString() + ".";

        // Calculate final payoff
        float totalReward = bonus1_reward + bonus2_reward + bonus3_reward + bonus4_reward;
        finalPay.GetComponent<Text>().text = "Your final payoff is $" + 
        (10+totalReward).ToString() + ", including $10 show-up fee and $" + totalReward.ToString() + " bonus.";
    }

    private void LoadReportedJson()
    {
        string jsonFile = File.ReadAllText(parameters.expDataJsonFilePath + "p" + participantID + "BUPractice.json");
        if (jsonFile != null)
        {
            reportedData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonFile);
        }
        else
        {
            Debug.LogError("posterior JSON file not found!");
        }
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


}
