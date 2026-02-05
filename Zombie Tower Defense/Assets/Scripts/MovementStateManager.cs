using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementStateManager : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private PlayerInput playerInput;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 0.2f;
    public float gravity = -12f;

    [Header("Anchor Settings")]
    //public Vector3 cameraOffset = new Vector3(0, 0, 0);

    private float verticalVelocity;
    [SerializeField] private float vertVelOffSet = 0;
    private float xRotation = 0f;

    private InputAction moveAction, lookAction; // to be changed
    private Vector3 moveInput, lookInput;
    public float vertIn, horIn;

    private void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        //playerCamera.localPosition = cameraOffset;

        Cursor.lockState = CursorLockMode.Locked;
        // moveAction.performed += PlayerMove;
        // lookAction.performed += PlayerLook;
    } 
    private void Update()
    {
        ReadInputs();
        PlayerMove();
        PlayerLook();
    }

    private void ReadInputs()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
    }
    private void PlayerMove()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 finalVelocity = (moveDirection * moveSpeed) + (Vector3.up * verticalVelocity);
        if (playerController.isGrounded && verticalVelocity < 0)
        {
            if (vertVelOffSet > 0)
            {
                float temp = vertVelOffSet * -1f;
                verticalVelocity = temp;
            }
            else
            {
                verticalVelocity = vertVelOffSet;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        playerController.Move(finalVelocity * Time.deltaTime);
    }

    private void PlayerLook()
    {
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);
        xRotation -= lookInput.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }
}
