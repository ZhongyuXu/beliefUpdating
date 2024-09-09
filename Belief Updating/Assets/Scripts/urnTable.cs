using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class urnTable : MonoBehaviour
{
    [System.Serializable]
    private class UrnEntry
    {
        public string urnName;
        public string prior;
        public List<string> composition;
        public float balls;
    }

    [System.Serializable]
    private class UrnEntryListData
    {
        public List<UrnEntry> urnEntries; // This matches the JSON structure
    }

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<UrnEntry> urnEntryList;
    private List<Transform> urnEntryTransformList;

    public string fileName; //json file name without .json extension

    void Start()
    {
        entryContainer = transform.Find("urnEntryContainer");
        entryTemplate = entryContainer.Find("urnEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        // Load the JSON data from the Resources folder
        urnEntryList = LoadUrnEntriesFromJson(fileName); // No need for .json extension
        urnEntryTransformList = new List<Transform>();

        foreach (UrnEntry urnEntry in urnEntryList)
        {
            CreateUrnEntryTransform(urnEntry, entryContainer, urnEntryTransformList);
        }
    }

    private List<UrnEntry> LoadUrnEntriesFromJson(string fileName)
    {
        // Load the JSON file from the Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName); // No need for .json extension
        if (jsonFile != null)
        {
            // Deserialize the JSON data into UrnEntryListData (which contains a List<UrnEntry>)
            UrnEntryListData listData = JsonUtility.FromJson<UrnEntryListData>(jsonFile.text);
            return listData.urnEntries;
        }
        else
        {
            Debug.LogError("JSON file not found: " + fileName);
            return new List<UrnEntry>();
        }
    }

    private void CreateUrnEntryTransform(UrnEntry urnEntry, Transform container, List<Transform> transformList)
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