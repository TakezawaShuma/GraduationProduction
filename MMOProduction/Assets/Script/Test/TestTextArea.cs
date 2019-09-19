using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTextArea: MonoBehaviour
{
    public string text = "TextField";
    private void OnGUI()
    {
        //GUI.TextArea(new Rect(10, 10, 100, 100), textToEdit);

        text = GUI.TextField(new Rect(10, 300, 100, 20), text);
        Debug.Log(text);
    }

}