using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // (opcional) pausar el juego si querés que el jugador no se mueva mientras usa la consola
            Time.timeScale = 0f;
        }
        else
        {
            Close();
        }

    }

    public void Close()
    {
        // Ocultar y bloquear el cursor nuevamente
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;

        camUi.HidePanel();
        consoleCanvas.SetActive(false);
    }
}
