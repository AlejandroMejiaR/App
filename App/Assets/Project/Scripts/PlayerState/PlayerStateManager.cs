using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;
    public PlayerStateData playerState; // Referencia al ScriptableObject
    private bool isFirstLoad = true; // Detecta si es la primera vez en la escena

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetProgreso();
        SaveCameraPositionIndex(0);
    }

    // Guardar estado del jugador, ahora incluyendo progreso
    public void SavePlayerState(GameObject player, int health, int score, int inventory)
    {
        int progreso = playerState.progresoMinijuegos; // Obtener el progreso actual para guardar
        playerState.SaveState(player.transform.position, player.transform.rotation, health, score, inventory, SceneManager.GetActiveScene().name, progreso);
    }

    // Guardar índice de posición de cámara
    public void SaveCameraPositionIndex(int index)
    {
        playerState.cameraPositionIndex = index;
    }

    // Cargar índice de posición de cámara
    public int LoadCameraPositionIndex()
    {
        return playerState.cameraPositionIndex;
    }

    // Cargar estado del jugador, ahora obteniendo el progreso también
    public void LoadPlayerState(GameObject player)
    {
        Vector3 position;
        Quaternion rotation;
        int health;
        int score;
        int inventory;
        string scene;
        int progreso; // Variable para cargar progreso

        playerState.LoadState(out position, out rotation, out health, out score, out inventory, out scene, out progreso);

        if (!isFirstLoad && scene == SceneManager.GetActiveScene().name)
        {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }

        isFirstLoad = false;
    }

    // Obtener el progreso actual
    public int GetProgreso()
    {
        return playerState.progresoMinijuegos;
    }

    // Incrementar el progreso en 1, con límite en 4
    public void IncrementarProgreso()
    {
        playerState.progresoMinijuegos++;
        if (playerState.progresoMinijuegos > 4)
            playerState.progresoMinijuegos = 4;
    }

    // Reiniciar progreso a 0
    public void ResetProgreso()
    {
        playerState.progresoMinijuegos = 0;
    }
}
