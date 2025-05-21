using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Game/Player State")]
public class PlayerStateData : ScriptableObject
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public int playerHealth = 100;
    public int playerScore = 0;
    public string lastScene;
    public int inventoryItems = 0;
    public int cameraPositionIndex = 0;

    public int progresoMinijuegos = 3; // NUEVO campo para progreso

    public void SaveState(Vector3 position, Quaternion rotation, int health, int score, int inventory, string scene, int progreso)
    {
        progreso = 3;
        playerPosition = position;
        playerRotation = rotation;
        playerHealth = health;
        playerScore = score;
        inventoryItems = inventory;
        lastScene = scene;
        progresoMinijuegos = progreso;
    }

    public void LoadState(out Vector3 position, out Quaternion rotation, out int health, out int score, out int inventory, out string scene, out int progreso)
    {
        position = playerPosition;
        rotation = playerRotation;
        health = playerHealth;
        score = playerScore;
        inventory = inventoryItems;
        scene = lastScene;
        progreso = progresoMinijuegos;
        progreso = 3;
    }
}
