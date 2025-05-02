using System.Collections;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    [Header("Referencias")]
    public Transform newPosition;      // La nueva posición del jugador
    public GameObject player;          // Referencia al jugador
    public Animator fadeAnimator;      // Animator del fade
    public string fadeTrigger = "Fade";// Nombre del trigger de animación

    [Header("Configuración")]
    public float teleportDelay = 1f;   // Tiempo antes del teletransporte
    public float detectionDistance = 2f;// Distancia de activación

    private bool canTeleport = true;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Verificar distancia entre jugador y puerta
        if (Vector3.Distance(transform.position, player.transform.position) < detectionDistance && canTeleport)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        canTeleport = false;

        // Activar animación de fade out
        fadeAnimator.SetTrigger(fadeTrigger);

        // Esperar mientras se completa el fade out
        yield return new WaitForSeconds(teleportDelay);

        // Mover al jugador a la nueva posición
        player.transform.position = newPosition.position;

        // Asegurar que la cámara siga al jugador
        if (mainCamera.orthographic)
        {
            // Para cámaras ortográficas (2D)
            mainCamera.transform.position = new Vector3(
                newPosition.position.x,
                newPosition.position.y,
                mainCamera.transform.position.z);
        }
        else
        {
            // Para cámaras en perspectiva (3D)
            mainCamera.transform.position = new Vector3(
                newPosition.position.x,
                mainCamera.transform.position.y,
                newPosition.position.z);
        }

        // Esperar un poco antes de permitir otro teletransporte
        yield return new WaitForSeconds(1f);
        canTeleport = true;
    }
}