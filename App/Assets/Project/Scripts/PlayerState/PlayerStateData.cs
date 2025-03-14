using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Game/Player State")]
public class PlayerStateData : ScriptableObject
{
    public Vector3 playerPosition;
    public Quaternion playerRotation; // Nueva variable para la rotación
    public int playerHealth = 100;
    public int playerScore = 0;
    public string lastScene;
    public int inventoryItems = 0; // Número de objetos recogidos

    public void SaveState(Vector3 position, Quaternion rotation, int health, int score, int inventory, string scene)
    {
        playerPosition = position;
        playerRotation = rotation; // Guardar la rotación
        playerHealth = health;
        playerScore = score;
        inventoryItems = inventory;
        lastScene = scene;
    }

    public void LoadState(out Vector3 position, out Quaternion rotation, out int health, out int score, out int inventory, out string scene)
    {
        position = playerPosition;
        rotation = playerRotation; // Cargar la rotación
        health = playerHealth;
        score = playerScore;
        inventory = inventoryItems;
        scene = lastScene;
    }
}

