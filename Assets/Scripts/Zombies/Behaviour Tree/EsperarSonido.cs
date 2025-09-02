using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsperarSonido : Node
{
    private Zombie zombie;
    private float tiempoEspera;
    private float temporizador;
    private bool patrullando;

    public EsperarSonido(Zombie z, float tiempo)
    {
        zombie = z;
        tiempoEspera = tiempo;
        temporizador = 0f;
        patrullando = false;
    }

    public override NodeState Evaluate()
    {
        if (!patrullando)
        {
            zombie.ActivarPatrulla();
            patrullando = true;
        }

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEspera)
        {
            zombie.DesactivarPatrulla();
            temporizador = 0f;
            patrullando = false;

            return NodeState.Success;
        }

        return NodeState.Running;
    }
}

