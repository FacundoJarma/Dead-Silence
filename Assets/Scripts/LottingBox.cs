using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LottingBox : MonoBehaviour, IInteractable
{
    public void Interact(){
        Destroy(gameObject);
    }
}
