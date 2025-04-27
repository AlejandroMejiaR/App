using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshair; // Referencia al punto de mira en el Canvas
    public Camera playerCamera; // Cámara del jugador
    public float maxDistance = 5f; // Distancia máxima para detectar objetos interactivos
    public LayerMask uiLayerMask;  // Máscara de capa para objetos UI (para ignorar el raycast cuando hay UI)

    void Start()
    {
        // Asegúrate de que el punto de mira sea visible
        if (crosshair != null)
        {
            crosshair.enabled = true; // Mostrar el punto de mira al inicio
        }
    }

    void Update()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                // Lanzar un raycast pero asegurarse de que no toque la UI
                if (Physics.Raycast(ray, out hit, maxDistance, ~uiLayerMask)) 
                {
                    // Si el rayo toca algo (un objeto interactivo), cambia el color del punto de mira
                    crosshair.color = Color.red;  // O cualquier otro color que indique que está mirando algo interactivo
                }
                else
                {
                    crosshair.color = Color.white;  // Color predeterminado cuando no se está mirando nada
                }
            }
        }
}
