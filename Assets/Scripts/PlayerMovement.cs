using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float crouchingSpeed = 3f;
    public float maxStamina = 100;
    float actualStamina = 0f;

    public delegate void OnStaminaChanged(float newStamina);
    public event OnStaminaChanged onStaminaChanged;

    public float normalHeight = 2f;
    public float crouchingHeight = 1.25f;

    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public Transform cameraHolder;

    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float acceleration = 10f;

    [HideInInspector]
    public bool canMove = true;

    private float currentHeight;
    private Vector3 currentCamPos;

    private bool isCrouching = false;
    private float currentSpeed = 0f;

    HeadBob playerCameraHeadBob;
    bool isFalling = false;

    AudioSource audioSource;

    public AudioClip footstepSound;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        actualStamina = maxStamina;
        characterController = GetComponent<CharacterController>();
        currentHeight = normalHeight;
        currentCamPos = cameraHolder.localPosition;
        playerCameraHeadBob = playerCamera.GetComponent<HeadBob>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool wantsToCrouch = Input.GetKey(KeyCode.LeftControl);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isRunning)
        {
            actualStamina -= 0.5f;
            if (actualStamina <= 0)
            {
                isRunning = false;
                actualStamina = 0;
            }
            onStaminaChanged?.Invoke(actualStamina);
        }
        else if (actualStamina < maxStamina)
        {
            actualStamina += 0.7f;
            onStaminaChanged?.Invoke(actualStamina);
        }

        if (wantsToCrouch)
        {
            isCrouching = true;
        }

        if (!wantsToCrouch && isCrouching)
        {
            Vector3 start = transform.position + Vector3.up * crouchingHeight;
            float checkDistance = normalHeight - crouchingHeight;

            if (!Physics.Raycast(start, Vector3.up, checkDistance + 0.1f))
            {
                isCrouching = false;
            }
        }

        float targetHeight = isCrouching ? crouchingHeight : normalHeight;
        Vector3 targetCamPos = new Vector3(0, targetHeight, 0);

        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 10);
        characterController.height = currentHeight;
        characterController.center = new Vector3(0, currentHeight / 2f, 0);

        currentCamPos = Vector3.Lerp(currentCamPos, targetCamPos, Time.deltaTime * 10);
        cameraHolder.localPosition = currentCamPos;

        float targetSpeed = isCrouching ? crouchingSpeed :
                            isRunning ? runningSpeed :
                            walkingSpeed;

        if (isCrouching && targetSpeed == walkingSpeed)
        {
            acceleration = 7f;
        }
        else if (isCrouching && currentSpeed > runningSpeed - 0.1f)
        {
            acceleration = 0.1f;
        }
        else
        {
            acceleration = 12f;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * acceleration);

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * inputY + right * inputX) * currentSpeed;

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            isFalling = true;
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            if (isFalling)
            {
                playerCameraHeadBob.doHeadBasicBob(5f, 1);
            }
            isFalling = false;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraHolder.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            playerCameraHeadBob.changeConfig(isCrouching ? "Crounching" : isRunning ? "Running" : "Walking");
            playerCameraHeadBob.isCrouchRunning = (isCrouching && currentSpeed > runningSpeed - 0.1f);
        }

        bool isMoving = Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f;

        if (characterController.isGrounded && isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                float intervalMultiplier = isCrouching ? 1.6f : isRunning ? 0.4f : 1f;
                footstepTimer = footstepInterval * intervalMultiplier;
            }
        }
        else
        {
            footstepTimer = footstepInterval; // evita el paso extra al frenar
        }

    }

    void PlayFootstepSound()
    {
        if (audioSource != null && footstepSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepSound);
        }
    }
}
