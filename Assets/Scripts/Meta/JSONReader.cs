using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class JSONReader
{
    public static List<T> ParseInput<T>(DialogueManager conversationManager, TextAsset jsonFile) 
    {
        List<T> list = JsonUtility.FromJson<List<T>>(jsonFile.text);
 
        foreach (var value in list)
        {
            list.Add(value);
        }
        return list;
    }
}