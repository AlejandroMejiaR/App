using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float horizontalLookLimit = 90f;

    private float xRotation = 0f;
    private Transform playerBody;
    private bool isCursorLocked = true; // Para verificar si el cursor está bloqueado o no

    void Start()
    {
        playerBody = transform.parent;

        if (playerBody == null)
        {
            Debug.LogError("No se encontró el objeto padre (Player) para la cámara.");
            return;
        }

        LockCursor();
    }   

    void Update()
    {
        // Solo permitir control de la cámara si el cursor está bloqueado
        if (isCursorLocked)
        {
            HandleCameraMovement();
        }
        else
        {
            // Si el cursor no está bloqueado, puedes agregar lógica para permitir el movimiento de la cámara si es necesario
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleCameraMovement()
    {
        // Obtén el movimiento del mouse para rotar la cámara
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación en el eje X (arriba/abajo) con límites para evitar que gire más de lo deseado
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -horizontalLookLimit, horizontalLookLimit);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación en el eje Y (izquierda/derecha) para el cuerpo del jugador
        float currentYRotation = playerBody.localEulerAngles.y;
        if (currentYRotation > 180f) currentYRotation -= 360f;

        float newYRotation = currentYRotation + mouseX;
        newYRotation = Mathf.Clamp(newYRotation, -horizontalLookLimit, horizontalLookLimit);
        playerBody.localRotation = Quaternion.Euler(0f, newYRotation, 0f);
    }

    // Método para bloquear el cursor (cuando el jugador está en control total)
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    // Método para desbloquear el cursor (cuando el panel está activo o si necesitas permitir la interacción)
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }
}
