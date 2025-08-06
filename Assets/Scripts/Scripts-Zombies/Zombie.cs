using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;       // El componente NavMeshAgent

    private bool persiguiendoJugador = false;
    private float tiempoTranscurrido = 0f;
    float tiempoAntesDePerseguir = 1f; // Tiempo antes de seguir al jugador

    public GameObject jugador;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();  // Obtener el agente
        if (jugador != null)
        {
            agente.SetDestination(jugador.transform.position);  // Mandarlo al destino
        }
        else
        {
            Debug.LogWarning("No se asignó un destino al zombie.");
        }
    }
    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        // Después de cierto tiempo, cambiar a persecución
        if (!persiguiendoJugador && tiempoTranscurrido >= tiempoAntesDePerseguir)
        {
            persiguiendoJugador = true;
        }

        // Si está en modo persecución, seguir al jugador
        if (persiguiendoJugador && jugador != null)
        {
            agente.SetDestination(jugador.transform.position);
        }
    }
}

    
   
   

    

   
    


