using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;
    public float tiltAmount = 10f; // inclinación máxima para mouse
    public float slideTilt = 10f;  // inclinación fija si se desliza
    public CharacterController controller;

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    private float timer;

    public bool isCrouchRunning = false;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        initialLocalRot = transform.localRotation;

        if (controller == null)
        {
            controller = GetComponentInParent<CharacterController>();
        }
    }

    void Update()
    {
        if (controller == null) return;

        float velocity = controller.velocity.magnitude;
        bool isMoving = velocity > 0.1f && controller.isGrounded;

        // HEADBOB MOVIMIENTO
        if (isMoving)
        {
            doHeadBasicBob(bobAmount, bobSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 8f);
            timer = 0;
        }

        // ROTACIÓN Z (TILTEO)
        float mouseX = Input.GetAxis("Mouse X");

        // Tilt por mouse + tilt por agacharse corriendo
        float mouseTilt = Mathf.Clamp(-mouseX * tiltAmount, -tiltAmount, tiltAmount);
        float slideExtraTilt = isCrouchRunning ? slideTilt : 0f;

        // Dirección del slide tilt debe coincidir con la del mouse
        float targetZRot = mouseTilt + (slideExtraTilt * Mathf.Sign(mouseTilt));

        // Aplicar rotación
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZRot);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * 6f);
    }

    public void changeConfig(string action)
    {
        switch (action)
        {
            case "Running":
                bobAmount = 0.15f;
                bobSpeed = 15f;
                break;

            case "Walking":
                bobAmount = 0.05f;
                bobSpeed = 8f;
                break;

            case "Crounching":
                bobAmount = 0.1f;
                bobSpeed = 8f;
                break;
        }
    }

    public void doHeadBasicBob(float amount, float speed)
    {
        timer += Time.deltaTime * speed;

        float bobY = Mathf.Sin(timer) * amount;
        float bobX = Mathf.Cos(timer * 0.5f) * amount * 0.5f;

        Vector3 offset = new Vector3(bobX, bobY, 0);
        offset.x = Mathf.Clamp(offset.x, -0.1f, 0.1f);
        offset.y = Mathf.Clamp(offset.y, -0.1f, 0.1f);

        transform.localPosition = new Vector3(
            initialLocalPos.x + offset.x,
            initialLocalPos.y + offset.y,
            initialLocalPos.z
        );
    }
}
