using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
     private NavMeshAgent agente;
    private NodoBT arbol;

    [Header("Jugador")]
    public GameObject jugador;

    [Header("Patrulla")]
    public Transform[] puntosPatrulla;

    private Vector3 ultimoSonido;
    private bool haySonido = false;
    private float tiempoEsperaSonido = 2f;
    private float temporizadorSonido = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Armamos el árbol:
        arbol = new Selector(
            new Sequence(
                new VerJugador(this),
                new PerseguirJugador(this)
            ),
            new Sequence(
                new HaySonido(this),
                new IrSonido(this),
                new EsperarSonido(this)
            ),
            new Patrullar(this)
        );
    }

    void Update()
    {
        // Calcula la dirección hacia el jugador, normalizada y con la longitud de 10 unidades
        Vector3 direccion = (jugador.transform.position - transform.position).normalized * 10f; 

        // Dibuja el rayo desde la posición del zombie más 1 unidad hacia arriba
        Debug.DrawRay(transform.position + Vector3.up, direccion, Color.red);
        arbol.Ejecutar();
    }

    // --------- FUNCIONES PARA LOS NODOS -----------

    public bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;
        float angulo = Vector3.Angle(transform.forward, direccion);

        if (angulo < 180f) // visión en cono
        {
            if (Physics.Raycast(transform.position + Vector3.up, direccion, out RaycastHit hit, 10f))
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.collider.gameObject.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    public void PerseguirJugador()
    {
        agente.SetDestination(jugador.transform.position);
    }

    public bool HaySonidoPendiente()
    {
        return haySonido;
    }

    public void IrAlSonido()
    {
        agente.SetDestination(ultimoSonido);
    }

    public bool EsperarEnSonido()
    {
        temporizadorSonido += Time.deltaTime;
        if (temporizadorSonido >= tiempoEsperaSonido)
        {
            haySonido = false;
            temporizadorSonido = 0f;
            return false; // termina la espera
        }
        return true; // sigue esperando
    }

    public void Patrullar()
    {
        if (!agente.hasPath && puntosPatrulla.Length > 0)
        {
            int indice = Random.Range(0, puntosPatrulla.Length);
            agente.SetDestination(puntosPatrulla[indice].position);
        }
    }
    public void VolverAPatrullar()
    {
        Patrullar();
    }
    // Llamado desde GestorSonidos
    public void IrAHaciaSonido(Vector3 pos)
    {
        ultimoSonido = pos;
        haySonido = true;
    }
}
