using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerJugador : NodoBT
{
    private Zombie zombie;

    public VerJugador(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        if (zombie.PuedeVerAlJugador())
            return true;
        return false;
    }
}
