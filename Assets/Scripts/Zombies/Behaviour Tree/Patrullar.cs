using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullar : NodoBT
{
     private Zombie zombie;

    public Patrullar(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        zombie.Patrullar();
        return true;
    }
}
