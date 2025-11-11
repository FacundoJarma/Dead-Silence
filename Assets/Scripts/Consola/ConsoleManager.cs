using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject consoleCanvas;
    [SerializeField] private CameraUIManager camUi;
    [SerializeField] private DoorsUIManager doorsUI;


    private Transform player;           // referencia al jugador
    private Vector3 initialPlayerPos;   // posición inicial cuando se abre la consola
    private bool consoleOpen = false;   // estado de la consola
    private float movementThreshold = 1f; // distancia máxima permitida antes de cerrar

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Si la consola está abierta, verificamos el movimiento del jugador
        if (consoleOpen && player != null)
        {
            float distance = Vector3.Distance(player.position, initialPlayerPos);
            if (distance > movementThreshold)
            {
                Close();
            }
        }
    }

    public void Interact()
    {
        bool isActive = !consoleCanvas.activeSelf;
        consoleCanvas.SetActive(isActive);

        if (isActive)
        {
            consoleOpen = true;
            initialPlayerPos = player.position; // guardar posición actual

            // Ocultar la mira y liberar el cursor
            GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        consoleOpen = false;

        // Restaurar cursor y cerrar interfaz
        GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        camUi.HidePanel();
        doorsUI.HidePanel();
        consoleCanvas.SetActive(false);
    }
}
