using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private NodoBT arbol;

    [Header("Patrulla Compu")]
    public Transform[] puntosCompu; 
    private bool patrullaCompuActiva = false;

    [Header("Jugador")]
    public GameObject jugador;

    [Header("Patrulla General")]
    public Transform[] puntosPatrulla;

    // --- Sonido ---
    private Vector3 ultimoSonido;
    private bool haySonido = false;
    private float tiempoEsperaSonido = 2f;
    private float temporizadorSonido = 0f;

    // --- Anti-bug / Anti-atascos ---
    private Vector3 ultimaPosicion;
    private float tiempoAtascado = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Configuración para evitar bugs
        agente.avoidancePriority = Random.Range(20, 80); 
        agente.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agente.autoRepath = true;
        agente.autoBraking = false;

        // Armamos el árbol:
        arbol = new Selector(
            new Sequence(
                new VerJugador(this),
                new PerseguirJugador(this)
            ),
            new Sequence(
                new HaySonido(this),
                new IrSonido(this),
                new EsperarSonido(this),
                new VolverAPatrullar(this)
            ),
            new Patrullar(this)
        );
    }

    void Update()
    {
        arbol.Ejecutar();

        // --- Anti-atascos ---
        if (Vector3.Distance(transform.position, ultimaPosicion) < 0.05f)
        {
            tiempoAtascado += Time.deltaTime;
            if (tiempoAtascado > 1.5f) // Si no se movió en 1.5s
            {
                RecalcularRuta();
                tiempoAtascado = 0f;
            }
        }
        else
        {
            tiempoAtascado = 0f;
        }

        ultimaPosicion = transform.position;

        // Dibujo del rayo de visión
        if (jugador != null)
        {
            Vector3 direccion = (jugador.transform.position - transform.position).normalized * 10f;
            Debug.DrawRay(transform.position + Vector3.up, direccion, Color.red);
        }
    }

    private void RecalcularRuta()
    {
        if (agente.hasPath)
        {
            agente.SetDestination(agente.destination); // recalcula el camino actual
        }
    }

    // --------- FUNCIONES PARA LOS NODOS -----------

    public bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;
        float angulo = Vector3.Angle(transform.forward, direccion);

        if (angulo < 90f) // visión en cono
        {
            if (Physics.Raycast(transform.position + Vector3.up, direccion, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("Player"))
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
            VolverAPatrullar();
            return false; 
        }

        return true; // sigue esperando
    }

    public void TerminarSonido()
    {
        haySonido = false;
        temporizadorSonido = 0f;
    }

    public void VolverAPatrullar()
    {
        Patrullar();
    }

    // --- Llamado desde Computadora ---
    public void IrAHaciaSonido(Vector3 pos)
    {
        // Offset aleatorio para que no se amontonen todos
        Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ultimoSonido = pos + offset;
        haySonido = true;
    }

    // --- Patrullaje ---
    public void Patrullar()
    {
        if (patrullaCompuActiva && puntosCompu.Length > 0)
        {
            if (!agente.hasPath)
            {
                IrAPuntoCompu();
            }
        }
        else if (puntosPatrulla.Length > 0)
        {
            if (!agente.hasPath)
            {
                int indice = Random.Range(0, puntosPatrulla.Length);
                agente.SetDestination(puntosPatrulla[indice].position);
            }
        }
    }

    private void IrAPuntoCompu()
    {
        int indice = Random.Range(0, puntosCompu.Length);
        agente.SetDestination(puntosCompu[indice].position);
    }

    public void AsignarPuntosCompu(Transform[] puntos)
    {
        puntosCompu = puntos;
    }
}
