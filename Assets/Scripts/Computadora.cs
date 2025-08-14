using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computadora : MonoBehaviour, IInteractable
{
    [Header("Configuración de sonido")]
    public float radioSonido = 10f; // Radio en el que los zombies escuchan
    public AudioClip sonidoComputadora; // Sonido que se reproduce al interactuar
    private AudioSource audioSource;

    [Header("Configuración de espera de zombies")]
    public float tiempoEsperaZombies = 2f; // Tiempo que los zombies esperan al llegar
    public float distanciaParada = 1f; // Distancia mínima a la computadora

    void Start()
    {
        // Intentar obtener AudioSource
        audioSource = GetComponent<AudioSource>();

        // Si no existe, lo creamos
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void Interact()
    {
        // Reproducir sonido
        if (sonidoComputadora != null)
        {
            audioSource.PlayOneShot(sonidoComputadora);
        }

        // Buscar zombies cercanos por tag y enviarlos hacia la compu
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject z in zombies)
        {
            float distancia = Vector3.Distance(z.transform.position, transform.position);
            if (distancia <= radioSonido)
            {
                Zombie scriptZombie = z.GetComponent<Zombie>();
                if (scriptZombie != null)
                {
                    StartCoroutine(EnviarZombie(scriptZombie));
                }
            }
        }
    }

    private IEnumerator EnviarZombie(Zombie z)
{
    // Calcular punto de destino a cierta distancia antes de la computadora
    Vector3 direccion = (transform.position - z.transform.position).normalized;
    Vector3 puntoDestino = transform.position - direccion * distanciaParada;

    // Mandar zombie hacia el punto calculado
    z.IrAHaciaSonido(puntoDestino);

    // Esperar a que llegue
    while (Vector3.Distance(z.transform.position, puntoDestino) > 5f)
    {
        yield return null;
    }

    // Esperar en el lugar
    yield return new WaitForSeconds(tiempoEsperaZombies);

    // Volver a patrullar
    z.VolverAPatrullar();
}

}
