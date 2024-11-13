using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class feedback : MonoBehaviour
{
    private Transform bonus1;

    void Start()
    {
        DefineVar();
        ChangeNumber();
    }

    private void DefineVar()
    {
        bonus1 = GameObject.Find("bonus1")?.transform;
    }

    private void ChangeNumber()
    {
        bonus1.GetComponent<Text>().text = "This is the first line \nThis is the second line";
    }

}
