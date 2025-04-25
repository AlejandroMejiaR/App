using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float horizontalLookLimit = 90f;
    
    private float xRotation = 0f;
    private Transform playerBody;
    
    void Start()
    {
        playerBody = transform.parent;
        
        if (playerBody == null)
        {
            Debug.LogError("No se encontró el objeto padre (Player) para la cámara.");
            return;
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (playerBody == null) return;
        
        if (IsPanelActive())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        float currentYRotation = playerBody.localEulerAngles.y;
        if (currentYRotation > 180f) currentYRotation -= 360f;
        
        float newYRotation = currentYRotation + mouseX;
        newYRotation = Mathf.Clamp(newYRotation, -horizontalLookLimit, horizontalLookLimit);
        
        playerBody.localRotation = Quaternion.Euler(0f, newYRotation, 0f);
    }
    
    private bool IsPanelActive()
    {
        Game3Manager gameManager = FindObjectOfType<Game3Manager>();
        if (gameManager != null)
        {
            return (gameManager.resultsPanel != null && gameManager.resultsPanel.activeSelf) || 
                   (gameManager.gameOverPanel != null && gameManager.gameOverPanel.activeSelf) || 
                   (gameManager.victoryPanel != null && gameManager.victoryPanel.activeSelf);
        }
        return false;
    }
}