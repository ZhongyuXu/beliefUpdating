using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class urnTable : MonoBehaviour
{
    public string instanceNameMaster = "BU1";
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<UrnInfo> urnEntryList;
    private List<Transform> urnEntryTransformList;
    private string jsonFilePath = parameters.jsonFilePath;
    private drawBalls drawBalls;
    private UrnWithStackedBalls urnWithStackedBalls;

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
        // Initialize drawBalls instance
        drawBalls = FindObjectOfType<drawBalls>();
        urnWithStackedBalls = FindObjectOfType<UrnWithStackedBalls>();

        // set instance name for other files
        drawBalls.instanceName = instanceNameMaster;
        urnWithStackedBalls.instanceName = instanceNameMaster;

        entryContainer = transform.Find("urnEntryContainer");
        entryTemplate = entryContainer.Find("urnEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        // Load the JSON data from the Resources folder
        urnEntryList = LoadUrnEntriesFromJson(); // No need for .json extension
        urnEntryTransformList = new List<Transform>();

        foreach (UrnInfo urnEntry in urnEntryList)
        {
            CreateUrnEntryTransform(urnEntry, entryContainer, urnEntryTransformList);
        }
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
            if (root.ContainsKey(instanceNameMaster))
            {
                // Return the ball draws list for the specified instance
                return root[instanceNameMaster].urnInfo;
            }
        }

        // If the instance name is not found, return an empty list and print an error message
        Debug.LogError("Instance name not found: " + instanceNameMaster);
        return new List<UrnInfo>();
    }

    private void CreateUrnEntryTransform(UrnInfo urnEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 16.25f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("urnText").GetComponent<Text>().text = urnEntry.urnName;
        entryTransform.Find("priorText").GetComponent<Text>().text = urnEntry.prior;
        entryTransform.Find("compositionText").GetComponent<Text>().text = string.Join(", ", urnEntry.composition);
        entryTransform.Find("totalBallsText").GetComponent<Text>().text = urnEntry.balls.ToString();

        transformList.Add(entryTransform);
    }

    void Update()
    {

    }
}