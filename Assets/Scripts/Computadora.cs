using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computadora : MonoBehaviour
{
    [Header("Configuración de sonido")]
    [SerializeField] int cantidadDeZombiesLlamar = 4;
    public float radioSonido = 10f; // Radio en el que los zombies escuchan
    public AudioClip sonidoComputadora; // Sonido que se reproduce al interactuar
    private AudioSource audioSource;

    [Header("Zombies asignados manualmente (opcional)")]
    [SerializeField] List<GameObject> zombiesAsignados = new List<GameObject>();


    [SerializeField] ConsoleManager consoleManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void PlaySound()
    {
        if (sonidoComputadora != null)
        {
            audioSource.PlayOneShot(sonidoComputadora);
        }

        List<GameObject> zombiesAUsar = new List<GameObject>();

        if (zombiesAsignados.Count > 0)
        {
            // Usar los zombies asignados manualmente
            zombiesAUsar = zombiesAsignados;
        }
        else
        {
            // Buscar zombies en rango como antes
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

            zombiesAUsar = zombiesEnRango;
        }

        for (int i = 0; i < Mathf.Min(cantidadDeZombiesLlamar, zombiesAUsar.Count); i++)
        {
            GameObject zombie = zombiesAUsar[i];
            Zombie scriptZombie = zombie.GetComponent<Zombie>();

            if (scriptZombie != null)
            {
                scriptZombie.IrAHaciaSonido(transform.position);
            }
        }

        consoleManager.Close();
    }
}
