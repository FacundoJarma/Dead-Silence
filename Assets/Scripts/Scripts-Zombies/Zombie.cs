using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform destino;          // El punto al que se moverá el zombie
    private NavMeshAgent agente;       // El componente NavMeshAgent

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();  // Obtener el agente
        if (destino != null)
        {
            agente.SetDestination(destino.position);  // Mandarlo al destino
        }
        else
        {
            Debug.LogWarning("No se asignó un destino al zombie.");
        }
    }
}
