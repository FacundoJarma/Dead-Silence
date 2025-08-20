using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsperarSonido : NodoBT
{
    private Zombie zombie;

    public EsperarSonido(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        return zombie.EsperarEnSonido();
    }
}
