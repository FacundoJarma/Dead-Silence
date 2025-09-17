using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolverAPatrullar : NodoBT
{
     private Zombie zombie;

    public VolverAPatrullar(Zombie z)
    {
        zombie = z;
    }

    public override bool Ejecutar()
    {
        zombie.VolverAPatrullar();
        return true; // siempre retorna true, es acción puntual
    }
}
