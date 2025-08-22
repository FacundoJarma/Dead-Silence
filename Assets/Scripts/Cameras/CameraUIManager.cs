using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUIManager : MonoBehaviour
{
    [SerializeField] GameObject camerasPanel;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            camerasPanel.SetActive(!camerasPanel.activeSelf);
        }
    }
}
