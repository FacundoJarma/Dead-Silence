using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computadora : MonoBehaviour, IInteractable
{

    [Header("Configuración de sonido")]
    [SerializeField] int cantidadDeZombiesLlamar = 4;
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

        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        List<GameObject> zombiesEnRango = new List<GameObject>();

        foreach (GameObject z in zombies)
        {
            float distancia = Vector3.Distance(z.transform.position, transform.position);
            if (distancia <= radioSonido)
            {
                zombiesEnRango.Add(z);
            }
        }

        zombiesEnRango.Sort((a, b) =>
            Vector3.Distance(a.transform.position, transform.position)
            .CompareTo(Vector3.Distance(b.transform.position, transform.position))
        );

        for (int i = 0; i < Mathf.Min(cantidadDeZombiesLlamar, zombiesEnRango.Count); i++)
        {
            GameObject zombie = zombiesEnRango[i];
            Zombie scriptZombie = zombie.GetComponent<Zombie>();

            if (scriptZombie != null)
            {
                scriptZombie.IrAHaciaSonido(transform.position);
            }
        }
    }

}
