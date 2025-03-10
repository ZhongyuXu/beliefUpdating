using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class questionsUrn : MonoBehaviour
{
    public string instanceName;
    private Transform urnSliderTemplate, urnSliderContainer;
    private List<UrnInfo> urnEntryList;
    public float questionH = 65f;
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
    void Start()
    {
        DefineVar();

        urnEntryList = LoadUrnEntriesFromJson();
        
        CreateUrnQuestions(urnEntryList, urnSliderContainer);
    }

        private List<UrnInfo> LoadUrnEntriesFromJson()
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
                return root[instanceName].urnInfo;
            }
        }

        // If the instance name is not found, return an empty list and print an error message
        Debug.LogError("Instance name not found in questionsUrns" + instanceName);
        return new List<UrnInfo>();
    }
    private void DefineVar()
    {
        urnTable = FindObjectOfType<urnTable>();
        instanceName = urnTable.instanceNameMaster;
        urnSliderContainer = transform.Find("urnSliderContainer");
        urnSliderTemplate = urnSliderContainer.Find("urnSliderTemplate");

        urnSliderTemplate.gameObject.SetActive(false);
    }


    private void CreateUrnQuestions(List<UrnInfo> urnEntryList, Transform container)
{
    for (int i = 0; i < urnEntryList.Count - 1; i++)
    {
        UrnInfo urnEntry = urnEntryList[i];
        Transform entryTransform = Instantiate(urnSliderTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(entryRectTransform.anchoredPosition.x, entryRectTransform.anchoredPosition.y -questionH * i);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("urnText").GetComponent<Text>().text = "Urn " + urnEntry.urnName;
    }
}

}

