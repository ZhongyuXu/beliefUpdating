using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRandomizer : MonoBehaviour
{
    // Scenes that shown upfront and not randomized
    public int introScenesCount = 1;
    // List of randomized scene indices
    private List<int> sceneBuildIndices = new List<int>();
    private int currentSceneIndex = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // Populate and shuffle the scene indices
        for (int i = introScenesCount; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            sceneBuildIndices.Add(i);
        }
        ShuffleList(sceneBuildIndices);
    }

    // Function to load the next scene in the order
    public void LoadNextScene()
    {
        if (currentSceneIndex < sceneBuildIndices.Count)
        {
            int buildIndex = sceneBuildIndices[currentSceneIndex];
            SceneManager.LoadScene(buildIndex);
            currentSceneIndex++;
        }
        else
        {
            Debug.Log("All scenes have been loaded.");
        }
    }

    // Shuffle method
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        Debug.Log("List shuffled: " + string.Join(", ", list));
    }
}