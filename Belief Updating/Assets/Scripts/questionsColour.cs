using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class questionsColour : MonoBehaviour
{
    public string instanceName;
    private Transform colourSliderTemplate, colourSliderContainer;
    private List<string> ballCompList, ballColours;
    public float questionH = 70f;
    private string jsonFilePath = parameters.jsonFilePath;
    private object uniqueColorList;

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
    void Start()
    {
        DefineVar();

        ballCompList = LoadBallColoursFromJson(); // e.g. ['3W', '2B', '1G']
        ballColours = ExtractBallColours(ballCompList); // e.g. ['White', 'Black', 'Green']

        CreateColourQuestions(ballColours, colourSliderContainer);
    }

        private List<string> LoadBallColoursFromJson()
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
                // Return the ball composition list for the specified instance
                List<string> ballComp = new List<string>();
                foreach (var urn in root[instanceName].urnInfo)
                {
                    ballComp.AddRange(urn.composition);
                }
                return ballComp;
            }
        }

        // If the instance name is not found, return an empty list and print an error message
        Debug.LogError("Instance name not found in questionsColour" + instanceName);
        return new List<string>();
    }
    private void DefineVar()
    {
        colourSliderContainer = transform.Find("colourSliderContainer");
        colourSliderTemplate = colourSliderContainer.Find("colourSliderTemplate");

        colourSliderTemplate.gameObject.SetActive(false);
    }

    private List<string> ExtractBallColours(List<string> ballCompList)
    {
        HashSet<string> uniqueColors = new HashSet<string>();
        foreach (string comp in ballCompList)
        {
            char colourCode = comp[comp.Length - 1]; // Assuming the last character represents the colour
            string colour = colourCode switch
            {
                'B' => "Black",
                'W' => "White",
                'P' => "Purple",
                'G' => "Green",
                _ => null,  // Add more colors if needed
            };
            uniqueColors.Add(colour);
        }
            List<string> uniqueColorList = new List<string>(uniqueColors);
        return uniqueColorList;
    }

    private void CreateColourQuestions(List<string> ballColours, Transform container)
{
    for (int i = 0; i < ballColours.Count; i++)
    {
        string ballCol = ballColours[i];
        Transform entryTransform = Instantiate(colourSliderTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(entryRectTransform.anchoredPosition.x, entryRectTransform.anchoredPosition.y -questionH * i);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("colourText").GetComponent<Text>().text = ballCol;
    }
}

}

