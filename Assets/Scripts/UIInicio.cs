using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInicio : MonoBehaviour
{
    // Llamado por el botón Jugar
    public void Jugar()
    {
        SceneManager.LoadScene(1); 
    }
}
