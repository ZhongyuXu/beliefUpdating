using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI; // For reading JSON files

public class DynamicSceneGenerator : MonoBehaviour
{
    public GameObject layoutPrefab; // Prefab for your layout
    public string jsonFileName; // JSON file to be loaded

    [System.Serializable]
    public class SceneData
    {
        public List<UrnEntry> urnEntries;
    }

    [System.Serializable]
    public class UrnEntry
    {
        public string urnName;
        public float prior;
        public List<string> composition;
        public float balls;
    }

    private void Start()
    {
        // Load the JSON data
        SceneData sceneData = LoadSceneData(jsonFileName);

        // Generate the scene based on the JSON data
        if (sceneData != null)
        {
            foreach (UrnEntry urnEntry in sceneData.urnEntries)
            {
                // Instantiate a new layout element for each entry
                GameObject layoutInstance = Instantiate(layoutPrefab, transform);

                // Set the properties of the layout (this assumes TextMeshPro fields in your prefab)
                layoutInstance.transform.Find("urnText").GetComponent<Text>().text = urnEntry.urnName;
                layoutInstance.transform.Find("priorText").GetComponent<Text>().text = urnEntry.prior.ToString();
                layoutInstance.transform.Find("compositionText").GetComponent<Text>().text = string.Join(", ", urnEntry.composition);
                layoutInstance.transform.Find("totalBallsText").GetComponent<Text>().text = urnEntry.balls.ToString();
            }
        }
        else
        {
            Debug.LogError("Failed to load scene data from JSON.");
        }
    }

    private SceneData LoadSceneData(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SceneData>(jsonContent);
        }
        else
        {
            Debug.LogError("JSON file not found: " + filePath);
            return null;
        }
    }
}