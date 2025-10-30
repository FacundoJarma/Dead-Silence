using UnityEngine;
using UnityEngine.AI;

public class Puerta : MonoBehaviour, IInteractable
{
    [Header("Configuración de apertura")]
    public float anguloApertura = 90f;
    public float velocidadApertura = 2f;
    public GameObject puerta;
    public bool abierta = false;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;
    private NavMeshObstacle obstaculo;

    [Header("Sonido")]
    public AudioSource audioSource; // 🎵 Arrastrá tu AudioSource aquí en el inspector

    void Start()
    {
        rotacionCerrada = puerta.transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles + new Vector3(0, anguloApertura, 0));

        obstaculo = GetComponent<NavMeshObstacle>();
        if (obstaculo == null)
            Debug.LogWarning("La puerta no tiene un componente NavMeshObstacle.");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>(); // Si está en el mismo objeto, lo toma automáticamente
    }

    public void Interact()
    {
        // 🎵 Reproducir el sonido al interactuar
        if (audioSource != null)
            audioSource.Play();

        StopAllCoroutines(); // Evita bugs si se interactúa rápido

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
        Quaternion inicio = puerta.transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * velocidadApertura;
            float suavizado = Mathf.SmoothStep(0f, 1f, t);
            puerta.transform.rotation = Quaternion.Slerp(inicio, destino, suavizado);
            yield return null;
        }

        puerta.transform.rotation = destino; // Asegura que termine exacta
    }
}
