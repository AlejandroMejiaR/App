using UnityEngine;

public class CameraPositionSetter : MonoBehaviour
{
    [Tooltip("Asigna aquí las posiciones (Empty) donde la cámara puede colocarse")]
    public Transform[] cameraPositions;

    private PlayerStateManager playerStateManager;
    private Camera mainCamera;

    void Start()
    {
        playerStateManager = PlayerStateManager.Instance;
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontró la cámara principal (Main Camera).");
            return;
        }

        int savedIndex = 0;

        if (playerStateManager != null)
        {
            savedIndex = playerStateManager.LoadCameraPositionIndex();
            if (savedIndex < 0 || savedIndex >= cameraPositions.Length)
            {
                savedIndex = 0; // fallback seguro
            }
        }

        ApplyCameraPosition(savedIndex);
    }

    void ApplyCameraPosition(int index)
    {
        if (cameraPositions == null || cameraPositions.Length == 0)
        {
            Debug.LogWarning("No se asignaron posiciones de cámara en CameraPositionSetter.");
            return;
        }

        if (index < 0 || index >= cameraPositions.Length)
        {
            Debug.LogWarning($"Índice de cámara fuera de rango: {index}");
            return;
        }

        mainCamera.transform.position = cameraPositions[index].position;
        mainCamera.transform.rotation = cameraPositions[index].rotation;
    }
}
