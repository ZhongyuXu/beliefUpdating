using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class urnTable : MonoBehaviour
{
    private class UrnEntry
    {
        public string urnName;
        public float prior;
        public List<string> composition;
        public float balls;
    }

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<UrnEntry> urnEntryList;
    private List<Transform> urnEntryTransformList;

    void Start()
    {
        entryContainer = transform.Find("urnEntryContainer");
        entryTemplate = entryContainer.Find("urnEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 16.25f;
        urnEntryList = new List<UrnEntry>()
        {
            new UrnEntry { 
                urnName = "Urn A",
                prior = 0.5f,
                composition = new List<string> { "1B", "3W"},
                balls = 4f,
            },
            new UrnEntry { 
                urnName = "Urn B",
                prior = 0.5f,
                composition = new List<string> { "3B", "1W"},
                balls = 4f,
            }
        };
        urnEntryTransformList = new List<Transform>();
        foreach (UrnEntry urnEntry in urnEntryList)
        {
            CreateUrnEntryTransform(urnEntry, entryContainer, urnEntryTransformList);
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
        entryTransform.Find("priorText").GetComponent<Text>().text = urnEntry.prior.ToString();
        entryTransform.Find("compositionText").GetComponent<Text>().text = string.Join(", ", urnEntry.composition);
        entryTransform.Find("totalBallsText").GetComponent<Text>().text = urnEntry.balls.ToString();

        transformList.Add(entryTransform);
    }

    void Update()
    {

    }
}
