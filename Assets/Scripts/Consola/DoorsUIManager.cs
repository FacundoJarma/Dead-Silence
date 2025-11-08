using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsUIManager : MonoBehaviour
{
    [Header("Panel a mostrar/ocultar")]
    public GameObject panel; // Asignalo desde el inspector

    void Start()
    {
        // Aseguramos que el panel comience oculto
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        // Si presiona Escape y el panel está visible → lo oculta
        if (panel != null && panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HidePanel();
        }
    }

    public void ShowPanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (panel != null)
            panel.SetActive(true);
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }
}
