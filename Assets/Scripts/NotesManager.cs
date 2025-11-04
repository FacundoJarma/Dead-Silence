using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] GameObject noteDisplay;
    [SerializeField] TextMeshProUGUI noteText;

    public void openOrClose(string note)
    {
        noteText.text = note;
        noteDisplay.SetActive(!noteDisplay.active);
    }

}
