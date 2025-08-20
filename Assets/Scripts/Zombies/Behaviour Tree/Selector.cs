using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : NodoBT
{
 private NodoBT[] nodos;

    public Selector(params NodoBT[] nodos)
    {
        this.nodos = nodos;
    }

    public override bool Ejecutar()
    {
        foreach (var nodo in nodos)
        {
            if (nodo.Ejecutar())
                return true; // si uno funciona, corta
        }
        return false;
    }
}
