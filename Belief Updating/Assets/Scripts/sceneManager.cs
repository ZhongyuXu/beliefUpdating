using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRandomizer : MonoBehaviour
{
    // Scenes that shown upfront and not randomized
    public int introScenesCount, outroScenesCount;
    // List of randomized scene indices
    private List<int> sceneBuildIndices = new List<int>();
    private int currentSceneIndex = 0;
    private float delayTime = 1.0f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // Populate and shuffle the scene indices
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
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
            // Load Fixation Cross before loading the next scene
            SceneManager.LoadScene("fixationCross");

            // After a delay, load the next scene
            StartCoroutine(ShowNextSceneWithDelay());
        }
        else
        {
            Debug.Log("All scenes have been loaded.");
        }
    }

    // Shuffle method
    void ShuffleList<T>(List<T> list)
    {
        for (int i = introScenesCount; i < (list.Count - outroScenesCount); i++)
        {
            int randomIndex = Random.Range(i, list.Count- outroScenesCount);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        Debug.Log("List shuffled: " + string.Join(", ", list));
    }

    private IEnumerator ShowNextSceneWithDelay()
    {
        yield return new WaitForSeconds(delayTime);

        int buildIndex = sceneBuildIndices[currentSceneIndex];
        SceneManager.LoadScene(buildIndex);
        currentSceneIndex++;
    }
}