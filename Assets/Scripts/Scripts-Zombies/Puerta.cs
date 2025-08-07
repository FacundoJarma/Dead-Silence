using UnityEngine;
using UnityEngine.AI;

public class Puerta : MonoBehaviour, IInteractable
{
    [Header("Apertura")]
    public float anguloApertura = 90f;
    public float velocidadApertura = 2f;

    private bool abierta = false;
    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    private NavMeshObstacle obstaculo;

    void Start()
    {
        // Guarda la rotación inicial y final
        rotacionInicial = transform.rotation;
        rotacionFinal = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));

        // Intenta encontrar el componente NavMeshObstacle
        obstaculo = GetComponent<NavMeshObstacle>();
        if (obstaculo == null)
        {
            Debug.LogWarning("La puerta no tiene un componente NavMeshObstacle.");
        }
    }

    public void Interact()
    {
        // Si ya está abierta, no hace nada
        if (abierta) return;

        abierta = true;

        // Desactiva el obstáculo del NavMesh para permitir paso
        if (obstaculo != null)
        {
            obstaculo.enabled = false;
        }

        // Empieza la animación de apertura
        StartCoroutine(AbrirPuerta());
    }

    private System.Collections.IEnumerator AbrirPuerta()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadApertura;
            transform.rotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, t);
            yield return null;
        }
    }
}


