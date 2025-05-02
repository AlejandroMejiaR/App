using UnityEngine;

[RequireComponent(typeof(InteractiveButton3D))]
public class ContinueButton3D : MonoBehaviour
{
    public Game3Manager gameManager;
    private InteractiveButton3D ib;

    void Awake()
    {
        if (ib == null)
        {
            ib = GetComponent<InteractiveButton3D>();
        }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<Game3Manager>();
        }

        // Eliminar cualquier suscripción anterior antes de añadir el listener
        ib.onButtonPressed.RemoveListener(gameManager.OnContinueCubePressed);
        ib.onButtonPressed.AddListener(gameManager.OnContinueCubePressed);
    }
}