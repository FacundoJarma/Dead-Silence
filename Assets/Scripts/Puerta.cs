using UnityEngine;
using UnityEngine.AI;

public class Puerta : MonoBehaviour, IInteractable
{
    public float anguloApertura = 90f;
    public float velocidadApertura = 2f;

    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    private NavMeshObstacle obstaculo;

    void Start()
    {
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));

        obstaculo = GetComponent<NavMeshObstacle>();
        if (obstaculo == null)
        {
            Debug.LogWarning("La puerta no tiene un componente NavMeshObstacle.");
        }
    }

    public void Interact()
    {
        StopAllCoroutines(); // Para evitar bugs si se interactúa rápido

        if (!abierta)
        {
            abierta = true;
            if (obstaculo != null)
                obstaculo.enabled = false; // Permitir pasar
            StartCoroutine(MoverPuerta(rotacionAbierta));
        }
        else
        {
            abierta = false;
            if (obstaculo != null)
                obstaculo.enabled = true; // Bloquear paso
            StartCoroutine(MoverPuerta(rotacionCerrada));
        }
    }

    System.Collections.IEnumerator MoverPuerta(Quaternion destino)
    {
        Quaternion inicio = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadApertura;
            float suavizado = Mathf.SmoothStep(0f, 1f, t); // Suaviza el movimiento
            transform.rotation = Quaternion.Slerp(inicio, destino, suavizado);
            yield return null;
        }

        transform.rotation = destino; // Asegura que termine exacta
    }
}
