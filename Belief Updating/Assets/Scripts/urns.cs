using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class urns : MonoBehaviour
{
    public string instanceName;
    private Transform urnContainer, ballContainer, textContainer;
    private Transform textTemplate, blackTemplate, whiteTemplate, purpleTemplate, greenTemplate, urnTemplate;
    private GameObject blackBallPrefab, whiteBallPrefab, purpleBallPrefab, greenBallPrefab;
    private List<UrnInfo> urnEntryList;

    // Ball layout settings
    private float dupScale = 1.39f;
    public float ballSpaceV = 16.25f;
    public float ballSpaceH = 16.25f;
    public float urnSpaceV = 0f;
    public float urnSpaceH = 200f;
    public int maxBallsPerRow = 3;    // Maximum balls per row
    private string jsonFilePath = parameters.jsonFilePath;
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

    private void Start() // This method is used as a Unity lifecycle method
    {
        DefineVar();
        // Load the JSON data from the Resources folder
        urnEntryList = LoadUrnEntriesFromJson(); // No need for .json extension

        CreateUrns();
        FillAllUrns();
    }

    private void DefineVar()
    {
        urnTable = FindObjectOfType<urnTable>();
        instanceName = urnTable.instanceNameMaster;
        
        urnContainer = transform.Find("urnContainer");
        urnTemplate = urnContainer.Find("urnTemplate");   
        ballContainer = urnContainer.Find("ballContainer");
        blackTemplate = ballContainer.Find("blackTemplate");
        whiteTemplate = ballContainer.Find("whiteTemplate");
        purpleTemplate = ballContainer.Find("purpleTemplate");
        greenTemplate = ballContainer.Find("greenTemplate");
        textContainer = urnContainer.Find("textContainer");
        textTemplate = textContainer.Find("textTemplate");


        blackTemplate.gameObject.SetActive(false);
        whiteTemplate.gameObject.SetActive(false);
        purpleTemplate.gameObject.SetActive(false);
        greenTemplate.gameObject.SetActive(false);
        urnTemplate.gameObject.SetActive(false);
        textTemplate.gameObject.SetActive(false);

        blackBallPrefab = blackTemplate.gameObject;
        whiteBallPrefab = whiteTemplate.gameObject;
        purpleBallPrefab = purpleTemplate.gameObject;
        greenBallPrefab = greenTemplate.gameObject;
    }
    private void CreateUrns()
    {
        // Create the urns. Omit the last entry since it's not an urn. It is the total urns row
        for (int i = 0; i < urnEntryList.Count - 1; i++)
        {
            // Create the urn background
            Transform urnTransform = Instantiate(urnTemplate, urnContainer);
            RectTransform urnRectTransform = urnTransform.GetComponent<RectTransform>();
            urnRectTransform.anchoredPosition = new Vector3(urnSpaceH * i, urnSpaceV * i, 10);
            urnTransform.gameObject.SetActive(true);
        }
    }

    private void FillAllUrns()
    {
        // Create the urns. Omit the last entry since it's not an urn. It is the total urns row
        for (int i = 0; i < urnEntryList.Count - 1; i++)
        {
            // Get data for one urn (i.e. Urn A)
            UrnInfo urnEntry = urnEntryList[i];

            // Create the text for the urn
            Transform textTransform = Instantiate(textTemplate, textContainer);
            RectTransform textRectTransform = textTransform.GetComponent<RectTransform>();
            textRectTransform.anchoredPosition = new Vector3(urnSpaceH * i * dupScale, urnSpaceV * i * dupScale, 0);
            textTransform.gameObject.SetActive(true);
            textTransform.Find("urnName").GetComponent<Text>().text = urnEntry.urnName;

            // Fill the urn with balls
            CreateBallsInOneUrn(urnEntry.composition, ballContainer, i);
        }
    }
    // This function generates balls based on the composition list from the JSON
    private void CreateBallsInOneUrn(List<string> composition, Transform container, int urnSequence)
    {
        int totalBalls = 0;

        foreach (string ballEntry in composition)
        {
            // Parse the number of balls and the color from the entry (e.g., "3B" -> 3, "B")
            int count = int.Parse(ballEntry[..^1]);
            char colorCode = ballEntry[^1];

            // Get the correct ball prefab based on the color code
            GameObject ballPrefab = GetBallPrefabFromColorCode(colorCode);
            Transform template = GetTemplateFromColorCode(colorCode);

            if (ballPrefab == null)
            {
                Debug.LogError("Invalid color code: " + colorCode);
                continue;
            }

            // Instantiate the specified number of balls for this color
            for (int i = 0; i < count; i++)
            {
                int row = totalBalls / maxBallsPerRow;
                int column = totalBalls % maxBallsPerRow;
                Transform entryTransform = Instantiate(template, container);
                RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

                float forwardZPosition = 0f;

                entryRectTransform.anchoredPosition = new Vector3(
                    urnSequence * urnSpaceH * dupScale + ballSpaceH * column, 
                    urnSequence * urnSpaceV * dupScale + ballSpaceV * row, 
                    forwardZPosition);

                entryTransform.gameObject.SetActive(true);
                // Write the code where the entrytransform is placed on the most forward plane of the scene
                totalBalls++;
            }
        }
    }

    // Function to get the correct ball prefab based on color code
    private GameObject GetBallPrefabFromColorCode(char colorCode)
    {
        GameObject ball = colorCode switch
        {
            'B' => blackBallPrefab,
            'W' => whiteBallPrefab,
            'P' => purpleBallPrefab,
            'G' => greenBallPrefab,
            _ => null,  // Add more colors if needed
        };
        return ball;
    }

    private Transform GetTemplateFromColorCode(char colorCode)
    {
        return colorCode switch
        {
            'B' => blackTemplate,
            'W' => whiteTemplate,
            'P' => purpleTemplate,
            'G' => greenTemplate,
            _ => null,  // Add more colors if needed
        };

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
        Debug.LogError("Instance name not found in urns.cs " + instanceName);
        return new List<UrnInfo>();
    }
}