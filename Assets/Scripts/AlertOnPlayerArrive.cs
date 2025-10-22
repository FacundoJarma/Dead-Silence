using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOnPlayerArrive : MonoBehaviour
{
    [SerializeField] string textToAlert;
    [SerializeField] bool hasBeenShowed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenShowed)
        {
            hasBeenShowed = true;
            FindObjectOfType<AlertManager>().DisplayInfo(textToAlert, 5f);

        }
    }
}
