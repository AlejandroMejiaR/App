using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractiveButton3D))]
public class SolutionSelector3D : MonoBehaviour
{
    [Tooltip("Referencia a tu Game3Manager (arrástralo en el inspector)")]
    public Game3Manager gameManager;

    [Tooltip("0 = Digital, 1 = Intermedia, 2 = Tradicional")]
    public int solutionIndex;

    private InteractiveButton3D interactiveButton;

    void Awake()
    {
        interactiveButton = GetComponent<InteractiveButton3D>();
        if (gameManager == null)
            gameManager = FindObjectOfType<Game3Manager>();
        
        interactiveButton.onButtonPressed.AddListener(OnPressed);
    }

    private void OnPressed()
    {
        gameManager.SelectSolution(solutionIndex);
    }
}
