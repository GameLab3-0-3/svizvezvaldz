using System.Net.NetworkInformation;
using UnityEngine;

public class Movement : MonoBehaviour
{
    InputMap inputActions;
    Rigidbody rb;
    [SerializeField] float speed;
    public static Movement instance;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputMap();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
            instance = this;
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    void FixedUpdate()
    {
        if(inputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            Movementfunction();
        }
    }
    private void Movementfunction()
    {
        Vector2 movementInput = inputActions.Player.Movement.ReadValue<Vector2>(); 
        Vector3 movement = rb.position + new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.fixedDeltaTime;
        rb.position = movement;

        Vector3 lookAt = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        transform.forward = lookAt;
    }
}
