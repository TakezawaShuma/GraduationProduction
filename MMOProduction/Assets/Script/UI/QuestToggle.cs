using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestToggle : MonoBehaviour
{
    public Packes.QuestMasterData data;
    [SerializeField]
    private Text questNameText = null;

    public Text GetText()
    {
        return questNameText;
    }
}
