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

    public void SavePlayerState(GameObject player, int health, int score, int inventory)
    {
        playerState.SaveState(player.transform.position, player.transform.rotation, health, score, inventory, SceneManager.GetActiveScene().name);
    }

    public void LoadPlayerState(GameObject player)
    {
        Vector3 position;
        Quaternion rotation;
        int health;
        int score;
        int inventory;
        string scene;

        playerState.LoadState(out position, out rotation, out health, out score, out inventory, out scene);

        if (!isFirstLoad && scene == SceneManager.GetActiveScene().name)
        {
            player.transform.position = position;
            player.transform.rotation = rotation; // Aplicar la rotación al jugador
        }

        isFirstLoad = false; // Marcar que ya se cargó la escena al menos una vez
        //Debug.Log($"Estado cargado: Posición {position}, Rotación {rotation.eulerAngles}, Vida {health}, Puntaje {score}, Inventario {inventory}, Última Escena {scene}");
    }
}
