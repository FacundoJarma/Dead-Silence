using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public float rayDistance = 3f;
    public LayerMask interactableLayer;

    public GameObject interactionIndicator;

    bool onInteraction = false;
    GameObject interactableObject;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                onInteraction = true;
                interactableObject = hit.collider.gameObject;

                // Mover el indicador al punto de impacto
                interactionIndicator.SetActive(true);
                interactionIndicator.transform.position = hit.point + hit.normal * 0.01f; // Un poco separado de la superficie
                interactionIndicator.transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90f, 0f, 0f);
            }
            else
            {
                onInteraction = false;
                interactableObject = null;
                interactionIndicator.SetActive(false);
            }
        }
        else
        {
            onInteraction = false;
            interactableObject = null;
            interactionIndicator.SetActive(false);
        }

        if (onInteraction && Input.GetKeyDown(KeyCode.E))
        {
            interactableObject.GetComponent<IInteractable>().Interact();
        }
    }
}
