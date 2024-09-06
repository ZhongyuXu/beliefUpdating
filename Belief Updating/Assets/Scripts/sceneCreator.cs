using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class SceneDuplicator : MonoBehaviour
{
    public string templateSceneName;
    public string jsonFileName;

    [System.Serializable]
    public class UrnEntry
    {
        public string urnName;
        public float prior;
        public List<string> composition;
        public float balls;
    }

    [System.Serializable]
    public class SceneData
    {
        public List<UrnEntry> urnEntries;
    }

    void Start()
    {
        // Load the template scene
        SceneManager.LoadScene(templateSceneName, LoadSceneMode.Additive);

        // Wait for the scene to load and modify the content
        StartCoroutine(LoadAndModifyScene());
    }

    IEnumerator LoadAndModifyScene()
    {
        yield return new WaitForSeconds(1f); // Wait for scene load

        // Load JSON data
        SceneData sceneData = LoadSceneData(jsonFileName);
        if (sceneData != null)
        {
            // Assuming you have layout prefabs in the scene, find them and update
            foreach (UrnEntry urnEntry in sceneData.urnEntries)
            {
                GameObject layout = GameObject.Find("LayoutPrefab"); // Example, adjust based on your setup
                layout.transform.Find("urnText").GetComponent<Text>().text = urnEntry.urnName;
                layout.transform.Find("priorText").GetComponent<Text>().text = urnEntry.prior.ToString();
                layout.transform.Find("compositionText").GetComponent<Text>().text = string.Join(", ", urnEntry.composition);
                layout.transform.Find("totalBallsText").GetComponent<Text>().text = urnEntry.balls.ToString();
            }
        }
    }

    private SceneData LoadSceneData(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
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