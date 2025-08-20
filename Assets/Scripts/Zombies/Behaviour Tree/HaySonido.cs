using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaySonido : NodoBT
{
    private Zombie zombie;

    public HaySonido(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        return zombie.HaySonidoPendiente();
    }
}
