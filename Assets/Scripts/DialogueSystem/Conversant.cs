using UnityEngine;
using System.Collections;

[System.Serializable]
public class Conversant
{
    internal string name;
    internal Sprite sprite;

    public Conversant(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}
