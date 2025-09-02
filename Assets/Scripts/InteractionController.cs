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
    Renderer interactableRenderer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (interactableObject != hit.collider.gameObject)
                {
                    ClearOutline(); 
                    interactableObject = hit.collider.gameObject;

                    // 🔹 Buscar el hijo con etiqueta "Model"
                    Transform model = interactableObject.transform.Find("Model");
                    if (model != null)
                    {
                        interactableRenderer = model.GetComponent<Renderer>();
                    }
                    else
                    {
                        interactableRenderer = interactableObject.GetComponent<Renderer>();
                    }

                    SetOutline(true);
                }

                onInteraction = true;

                // Mostrar indicador
                interactionIndicator.SetActive(true);
                Vector3 indicatorPos = interactableObject.transform.position;
                indicatorPos.y += interactableObject.transform.localScale.y / 2 + 1;
                interactionIndicator.transform.position = indicatorPos;

                Vector3 lookDirection = transform.position - interactionIndicator.transform.position;
                interactionIndicator.transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(90f, 0f, 0f);
            }
        }
        else
        {
            ClearOutline();
            onInteraction = false;
            interactableObject = null;
            interactableRenderer = null;
            interactionIndicator.SetActive(false);
        }

        // Interacción
        if (onInteraction && Input.GetKeyDown(KeyCode.E))
        {
            interactableObject.GetComponent<IInteractable>().Interact();
        }
    }

    void SetOutline(bool state)
    {
        if (interactableRenderer != null)
            interactableRenderer.material.SetFloat("_Outline", state ? 0.05f : 0f);
    }

    void ClearOutline()
    {
        if (interactableRenderer != null)
            interactableRenderer.material.SetFloat("_Outline", 0f);
    }
}
