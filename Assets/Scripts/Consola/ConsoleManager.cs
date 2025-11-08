using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject consoleCanvas;

    [SerializeField] CameraUIManager camUi;
    public void Interact()
    {
        bool isActive = !consoleCanvas.activeSelf;
        consoleCanvas.SetActive(isActive);

        if (isActive)
        {
            // Mostrar y desbloquear el cursor
            GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>().enabled = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // (opcional) pausar el juego si querés que el jugador no se mueva mientras usa la consola
        }
        else
        {
            Close();
        }

    }


    public void Close()
    {
        // Ocultar y bloquear el cursor nuevamente
        GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>().enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camUi.HidePanel();
        consoleCanvas.SetActive(false);
    }
}
