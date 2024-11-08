using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string uniqueID;
    public string dialogueText;
    public string[] children;
    public Rect rect = new Rect(0, 0, 200, 200);
}