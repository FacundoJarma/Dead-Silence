using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrSonido : NodoBT
{
  private Zombie zombie;

    public IrSonido(Zombie zombie)
    {
        this.zombie = zombie;
    }

    public override bool Ejecutar()
    {
        zombie.IrAlSonido();
        return true;
    }
}
