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

        // Buscar zombies cercanos y avisarles
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject zGO in zombies)
        {
            float distancia = Vector3.Distance(transform.position, zGO.transform.position);
            Debug.Log(distancia);
            if (distancia <= radioSonido)
            {
                Zombie z = zGO.GetComponent<Zombie>();
                if (z != null)
                {
                    z.IrAHaciaSonido(transform.position);
                }
            }
        }
    }
}
