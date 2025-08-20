using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerseguirJugador : NodoBT
{
    private Zombie zombie;

    public PerseguirJugador(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        zombie.PerseguirJugador();
        return true;
    }
}
