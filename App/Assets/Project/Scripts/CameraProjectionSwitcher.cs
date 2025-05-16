using UnityEngine;

public class CameraProjectionSwitcher : MonoBehaviour
{
    [Header("Referencia a la cámara")]
    public Camera targetCamera;

    [Header("Referencia al objeto para comparar posición")]
    public Transform targetObject;

    void Update()
    {
        if (targetCamera == null || targetObject == null)
            return;

        // Compara posiciones exactas (puedes ajustar con tolerancia si quieres)
        if (targetCamera.transform.position == targetObject.position)
        {
            targetCamera.orthographic = false; // perspectiva
        }
        else
        {
            targetCamera.orthographic = true;  // ortográfica
        }
    }
}
