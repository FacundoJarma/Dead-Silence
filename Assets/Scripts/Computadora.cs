using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computadora : MonoBehaviour, IInteractable
{
    public float radioSonido = 10f; // Radio en el que los zombies escuchan
    public AudioClip sonidoComputadora; // Sonido que se reproduce al interactuar
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Interact()
    {
        // Reproducir sonido
        if (sonidoComputadora != null)
        {
            audioSource.PlayOneShot(sonidoComputadora);
        }

        // Avisar a los zombies que hubo ruido
        if (GestorSonidos.instancia != null)
        {
            GestorSonidos.instancia.EmitirSonido(transform.position, radioSonido);
        }
        else
        {
            Debug.LogWarning("No se encontró la instancia de GestorSonidos en la escena.");
        }
    }
}
