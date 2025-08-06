using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;

    public GameObject jugador;
    public LayerMask capaObstaculos;     // Obstáculos que bloquean la vista
    public LayerMask capaJugador;        // Layer del jugador

    public float distanciaVision = 10f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (jugador == null)
        {
            Debug.LogWarning("No se asignó el jugador al zombie.");
        }
    }

    void Update()
    {
        if (PuedeVerAlJugador())
        {
            agente.SetDestination(jugador.transform.position);
        }
        else
        {
            agente.ResetPath(); // Detiene al zombie si no ve al jugador
        }
    }

    bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 origen = transform.position + Vector3.up; // elevar raycast
        Vector3 direccion = (jugador.transform.position - origen).normalized;

        // Lanza un raycast y verifica si el primer objeto golpeado es el jugador
        if (Physics.Raycast(origen, direccion, out RaycastHit hit, distanciaVision, capaJugador | capaObstaculos))
        {
            // Comprobamos si el objeto impactado pertenece a la capa del jugador
            if (((1 << hit.collider.gameObject.layer) & capaJugador) != 0)
            {
                return true;
            }
        }

        return false;
    }
}

