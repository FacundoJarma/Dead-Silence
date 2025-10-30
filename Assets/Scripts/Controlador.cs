using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour
{
    void Start()
    {
        // Detectar qué escena está activa
        string escenaActual = SceneManager.GetActiveScene().name;

        if (escenaActual == "Escena 1era version") // 👈 pon el nombre exacto de tu escena principal
        {
            // Configuración de juego (cursor oculto y bloqueado)
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (escenaActual == "Inicio-Scene") // 👈 pon el nombre exacto de la escena de muerte
        {
            // Configuración de menú (cursor visible y libre)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}