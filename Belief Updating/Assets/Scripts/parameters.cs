using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class parameters : System.Object
{
    public static string inputJsonFilePath = Application.streamingAssetsPath + "/input.json";
    public static string posteriorsJsonFilePath = Application.streamingAssetsPath + "/obj_posteriors.json";
    public static string expDataJsonFilePath = "Assets/participantData/";
    public static string demographicDataCSVFilePath = "Assets/participantData/demographicData/";
}