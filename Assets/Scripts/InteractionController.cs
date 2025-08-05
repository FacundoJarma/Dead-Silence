using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{    
    // Update is called once per frame
    void Update()
    {
         RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2.5f))

        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2.5f, Color.yellow); 
            Debug.Log("Did Hit"); 
        }
        else
        { 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2.5f, Color.white); 
            Debug.Log("Did not Hit"); 
        }

    }
}
