using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;
    public PlayerStateData playerState; // Referencia al ScriptableObject

    private bool isFirstLoad = true;
    private bool hasStartedFinalSequence = false;  // Para evitar múltiples llamadas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetProgreso();
        SaveCameraPositionIndex(0);
    }

    // Carga el estado guardado y si ya completó los 4 minijuegos, inicia la espera
    public void LoadPlayerState(GameObject player)
    {
        Vector3 position;
        Quaternion rotation;
        int health, score, inventory;
        string sceneName;
        int progreso;

        playerState.LoadState(
            out position, out rotation,
            out health, out score,
            out inventory, out sceneName,
            out progreso
        );

        if (!isFirstLoad && sceneName == SceneManager.GetActiveScene().name)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }

        isFirstLoad = false;

        if (playerState.progresoMinijuegos >= 4 && !hasStartedFinalSequence)
            StartCoroutine(DelayAndLoadFinal());
    }

    // Se llama al terminar cada minijuego
    public void IncrementarProgreso()
    {
        playerState.progresoMinijuegos = Mathf.Min(playerState.progresoMinijuegos + 1, 4);
        if (playerState.progresoMinijuegos >= 4 && !hasStartedFinalSequence)
            StartCoroutine(DelayAndLoadFinal());
    }

    // Corutina que espera 7 segundos antes de cargar 'Final'
    private IEnumerator DelayAndLoadFinal()
    {
        hasStartedFinalSequence = true;
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene("Final");
    }

    public int GetProgreso()
    {
        return playerState.progresoMinijuegos;
    }

    public void SavePlayerState(GameObject player, int health, int score, int inventory)
    {
        int progreso = playerState.progresoMinijuegos;
        playerState.SaveState(
            player.transform.position,
            player.transform.rotation,
            health,
            score,
            inventory,
            SceneManager.GetActiveScene().name,
            progreso
        );
    }

    public void SaveCameraPositionIndex(int index)
    {
        playerState.cameraPositionIndex = index;
    }

    public int LoadCameraPositionIndex()
    {
        return playerState.cameraPositionIndex;
    }

    // Reinicia el conteo de minijuegos completados
    public void ResetProgreso()
    {
        playerState.progresoMinijuegos = 0;
        hasStartedFinalSequence = false;
    }
}




