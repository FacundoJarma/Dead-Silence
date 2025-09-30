using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computadora : MonoBehaviour, IInteractable
{
    
    [Header("Configuración de sonido")]
    public float radioSonido = 10f; // Radio en el que los zombies escuchan
    public AudioClip sonidoComputadora; // Sonido que se reproduce al interactuar
    private AudioSource audioSource;

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
            Debug.Log(z);
            Debug.Log(distancia);

            if (distancia <= radioSonido)
            {
                Zombie scriptZombie = z.GetComponent<Zombie>();
               
                scriptZombie.IrAHaciaSonido(transform.position);

            }
        }
    }

}
