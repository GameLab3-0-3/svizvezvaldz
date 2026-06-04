using UnityEngine;

// Ricordati di rinominare anche il file in "CameraController.cs" nel tuo progetto!
public class CameraController : MonoBehaviour 
{
    [Header("Look Settings")]
    [SerializeField] float mouseSensitivity = 100f; // Ho alzato il valore di base per compensare il deltaTime
    
    private Transform playerBody;
    private float xRotation = 0f;

    void Start()
    {
        // Nasconde il cursore e lo blocca al centro dello schermo
        Cursor.lockState = CursorLockMode.Locked;

        if (transform.parent != null)
            playerBody = transform.parent;
        else
            Debug.LogError("La Camera non è figlia del Player! Trascinala dentro l'oggetto Player.");
    }

    // Usa Update invece di FixedUpdate per l'input e la rotazione
    void Update() 
    {
        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        if (playerBody == null) return;

        // Passiamo a Time.deltaTime invece di Time.fixedDeltaTime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applica la rotazione verticale alla telecamera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Applica la rotazione orizzontale al corpo del giocatore
        playerBody.Rotate(Vector3.up * mouseX);
    }
}