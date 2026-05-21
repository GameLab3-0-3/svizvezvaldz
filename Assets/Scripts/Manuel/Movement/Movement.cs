using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    InputMap inputmap;
    Rigidbody rb;
    [Header("Movement Stats")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 9f;
    private float currentSpeed;
    [Header("Stamina Settings")]
    [SerializeField] float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] float staminaDrain = 20f;   // Stamina tolta al secondo correndo
    [SerializeField] float staminaRegen = 15f;   // Stamina rigenerata al secondo
    [SerializeField] UnityEngine.UI.Image staminaImage; // Barra della stamina UI
    public static Movement instance;
    void Awake()
    {
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
        
        rb = GetComponent<Rigidbody>();
        inputmap = new InputMap();

        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;

        // Configurazione fisica per il primo persona
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Nasconde e blocca il cursore
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnEnable()
    {
        inputmap.Enable();
    }
    void OnDisable()
    {
        inputmap.Disable();
    }
    void Update()
    {
        HandleSprintAndStamina();
    }
    void FixedUpdate()
    {
        Movements();
    }
    void HandleSprintAndStamina()
    {
        Vector2 movementInput = inputmap.Player.Movement.ReadValue<Vector2>();       
        bool isSprintPressed = inputmap.Player.Sprint.ReadValue<float>() > 0; 
        bool isMoving = movementInput != Vector2.zero;
        if (isSprintPressed && isMoving && currentStamina > 0)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            currentSpeed = walkSpeed;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }
        }
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        if (staminaImage != null)
        {
            staminaImage.fillAmount = currentStamina / maxStamina;
        }
    }
    void Movements()
    {
        Vector2 movementInput = inputmap.Player.Movement.ReadValue<Vector2>();
        if (movementInput == Vector2.zero)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        moveDirection.Normalize();
        Vector3 targetVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}