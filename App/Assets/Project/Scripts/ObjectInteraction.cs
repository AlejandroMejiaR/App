using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject interactionUI; // UI de interacción sobre el objeto
    public string sceneToLoad = "MiniGame01"; // Nombre de la escena a cargar
    private bool isNear = false;
    private Camera mainCamera;
    private CanvasGroup canvasGroup; // Para el efecto de Fade

    void Start()
    {
        if (interactionUI)
        {
            canvasGroup = interactionUI.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                canvasGroup.alpha = 0; // Iniciar invisible
            }
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }

        // Hacer que la UI siempre mire hacia la cámara
        if (interactionUI)
        {
            interactionUI.transform.LookAt(mainCamera.transform);
            interactionUI.transform.Rotate(0, 180, 0); // Voltear para que no esté al revés
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            StartCoroutine(FadeUI(1)); // Fade in (aparecer)
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            StartCoroutine(FadeUI(0)); // Fade out (desaparecer)
        }
    }

    void OnMouseDown()
    {
        if (Application.isMobilePlatform)
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    void SavePlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            PlayerStateManager.Instance.SavePlayerState(player, 100, 0, 0); // Guardar estado con rotación
        }
    }


    // Corrutina para hacer el Fade Out antes de cambiar la escena
    IEnumerator FadeOutAndLoadScene()
    {
        if (canvasGroup)
        {
            float duration = 0.5f;
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
                yield return null;
            }

            canvasGroup.alpha = 0; // Asegurar que llega al valor exacto
        }

        SavePlayerState(); // Guardar la posición del jugador
        SceneManager.LoadScene(sceneToLoad); // Cargar la nueva escena después del Fade Out
    }

    // Corrutina para hacer el Fade In y Fade Out suavemente
    IEnumerator FadeUI(float targetAlpha)
    {
        if (canvasGroup)
        {
            float duration = 0.5f;
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha; // Asegurar que llega al valor exacto
        }
    }
}
