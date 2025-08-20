using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : NodoBT
{
    private NodoBT[] nodos;

    public Sequence(params NodoBT[] nodos)
    {
        this.nodos = nodos;
    }

    public override bool Ejecutar()
    {
        foreach (var nodo in nodos)
        {
            if (!nodo.Ejecutar())
                return false; // si falla uno, corta
        }
        return true;
    }
}
