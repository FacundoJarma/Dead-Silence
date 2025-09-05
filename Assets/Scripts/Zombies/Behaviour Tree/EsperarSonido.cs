using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsperarSonido : NodoBT
{
    private Zombie zombie;
    private float tiempoEspera;
    private float temporizador;
    private bool patrullando;

    public EsperarSonido(Zombie z, float tiempo = 5f)
    {
        zombie = z;
        tiempoEspera = tiempo;
        temporizador = 0f;
        patrullando = false;
    }

    public override bool Ejecutar()
    {
        if (!patrullando)
        {
            zombie.ActivarPatrullaCompu();
            patrullando = true;
        }

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEspera)
        {
            zombie.DesactivarPatrullaCompu();
            temporizador = 0f;
            patrullando = false;

            return true;
        }
        return true;

    }
}

