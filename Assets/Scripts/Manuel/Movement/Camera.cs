using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] float mouseSensitivity = 15f;   
    private Transform playerBody;
    private float xRotation = 0f;
    void Start()
    {
        if (transform.parent != null)
            playerBody = transform.parent;
        else
            Debug.LogError("La Camera non è figlia del Player! Trascinala dentro l'oggetto Player.");
    }
    void FixedUpdate()
    {
        HandleCameraRotation();
    }
    void HandleCameraRotation()
    {
        if (playerBody == null) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}