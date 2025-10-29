using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [Header("Escenas en las que el cursor debe estar visible")]
    [Tooltip("Nombres exactos de las escenas donde el cursor debe mostrarse (ej. Menú, GameOver, Inicio).")]
    public string[] escenasConCursorVisible;

    void Start()
    {
        ActualizarCursor(); // Llamamos una vez al iniciar
        SceneManager.sceneLoaded += OnSceneLoaded; // Nos suscribimos al evento cuando cambia la escena
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Importante para evitar errores al cambiar de escena
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActualizarCursor();
    }

    void ActualizarCursor()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        bool necesitaCursor = false;

        // Verificamos si la escena actual está en la lista
        foreach (string escena in escenasConCursorVisible)
        {
            if (escenaActual == escena)
            {
                necesitaCursor = true;
                break;
            }
        }

        if (necesitaCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Debug.Log("Cursor actualizado: " + (necesitaCursor ? "Visible" : "Oculto") + " en escena " + escenaActual);
    }
}
