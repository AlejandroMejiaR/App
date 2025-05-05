using UnityEngine;
using UnityEngine.UI;  // Importar para usar UI
using UnityEngine.SceneManagement;  // Para manejar las escenas
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CubeGameManager2 : MonoBehaviour
{
    public static CubeGameManager2 Instance;

    [Header("Referencias")]
    public List<CubeController2> allCubes = new List<CubeController2>();
    public List<SafeZone2> safeZones = new List<SafeZone2>();
    public GameObject winMessagePanel; // Referencia al panel de victoria
    public GameObject gameOverPanel;   // Referencia al panel de game over
    public TextMeshProUGUI timeText;  // Texto del tiempo restante
    public Button continueButton;     // Botón de continuar en Game Over

    [Header("Configuración")]
    public float verificationDelay = 1f;
    public float victoryDelay = 1f;
    public float[] levelTimes;        // Array de tiempos para cada nivel
    
    private float timeRemaining;
    private bool gameOver = false;
    private bool isVerificationCompleted = false; // Para controlar la verificación

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (winMessagePanel != null)
            winMessagePanel.SetActive(false); // Aseguramos que el panel esté oculto

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Aseguramos que el panel de game over esté oculto
    }

    private void Start()
    {
        // Configurar el tiempo según el nivel actual
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "TutorialMinijuego4")
        {
            timeRemaining = levelTimes[0]; // Tiempo para Tutorial
            if (timeText != null) timeText.gameObject.SetActive(false); // Ocultar el texto del tiempo en el tutorial
        }
        else if (currentScene == "Minijuego4Level1")
        {
            timeRemaining = levelTimes[1]; // Tiempo para Minijuego4Level1
            if (timeText != null) timeText.gameObject.SetActive(true); // Asegurarse de que se vea el tiempo en el nivel
        }
        else
        {
            timeRemaining = 30f; // Tiempo por defecto
        }

        StartCoroutine(TimerCountdown());

        // Configurar el botón de continuar
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(RestartLevel);
        }
    }

    private void Update()
    {
        if (gameOver) return; // Si ya se acabó el juego, no actualizar el temporizador

        // Actualizamos el texto del temporizador
        if (timeText != null)
        {
            timeText.text = "Tiempo: " + Mathf.Ceil(timeRemaining).ToString() + "s";
        }

        // Verificamos si el tiempo se agotó
        if (timeRemaining <= 0f && !isVerificationCompleted)
        {
            GameOver();
        }

        // Verificación continua de cubos y zonas
        if (!isVerificationCompleted)
        {
            CheckAllCubes();  // Verificar si todos los cubos están en su lugar
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (timeRemaining > 0f && !gameOver)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }
    }

    public void CheckAllCubes()
    {
        if (isVerificationCompleted) return;

        bool allPlaced = true;
        foreach (CubeController2 cube in allCubes)
        {
            if (!cube.isInSafeZone)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            // Resetear el estado de verificación al iniciar una nueva
            isVerificationCompleted = false; 
            StartCoroutine(DelayedVerification());
        }
    }

    private IEnumerator DelayedVerification()
    {
        isVerificationCompleted = true;
        yield return new WaitForSeconds(1f);
        
        bool allCorrect = true;
        bool allZonesOccupied = true;

        // Primera pasada: Verificar estado de todas las zonas
        foreach (SafeZone2 zone in safeZones)
        {
            if (zone.currentCubes.Count == 0)
            {
                allZonesOccupied = false;
                zone.GetZoneRenderer().material.color = zone.neutralColor;
                continue;
            }

            bool zoneCorrect = true;
            foreach (CubeController2 cube in zone.currentCubes)
            {
                if (cube.cubeID != zone.zoneID)
                {
                    zoneCorrect = false;
                    allCorrect = false;
                    break;
                }
            }
            
            zone.GetZoneRenderer().material.color = zoneCorrect ? zone.correctColor : zone.wrongColor;
        }

        // Condiciones para continuar:
        // 1. Todas las zonas deben tener al menos un cubo
        // 2. Todos los cubos deben estar en la zona correcta
        if (!allZonesOccupied || !allCorrect)
        {
            yield return new WaitForSeconds(0.5f);
            
            // Resetear solo las zonas incorrectas o vacías
            foreach (SafeZone2 zone in safeZones)
            {
                bool shouldReset = false;
                
                // Verificar si la zona está vacía o tiene cubos incorrectos
                if (zone.currentCubes.Count == 0)
                {
                    shouldReset = true;
                }
                else
                {
                    foreach (CubeController2 cube in zone.currentCubes)
                    {
                        if (cube.cubeID != zone.zoneID)
                        {
                            shouldReset = true;
                            break;
                        }
                    }
                }

                if (shouldReset)
                {
                    List<CubeController2> cubesToRemove = new List<CubeController2>(zone.currentCubes);
                    
                    foreach (CubeController2 cube in cubesToRemove)
                    {
                        cube.Unlock();
                        cube.ResetPosition();
                    }
                    
                    zone.ResetZone();
                }
            }
            
            isVerificationCompleted = false;
            yield break;
        }
        
        // Bloquear todos los cubos y mostrar victoria
        foreach (SafeZone2 zone in safeZones)
        {
            foreach (CubeController2 cube in zone.currentCubes)
            {
                cube.LockInPlace();
            }
        }
        
        yield return new WaitForSeconds(1f);
        ShowVictoryMessage();
    }

    private IEnumerator VerifyCubesCoroutine()
    {
        yield return new WaitForSeconds(verificationDelay);

        bool allCorrect = true;

        // Iterar sobre cada zona segura y verificar todos los cubos en ella
        foreach (SafeZone2 zone in safeZones)
        {
            bool zoneCorrect = true;
            // Verificar todos los cubos dentro de la zona
            foreach (CubeController2 cube in zone.currentCubes)
            {
                if (cube.cubeID != zone.zoneID)
                {
                    zoneCorrect = false;
                    break;
                }
            }

            // Cambiar el color de la zona en función de si todos los cubos son correctos
            zone.GetZoneRenderer().material.color = zoneCorrect ? zone.correctColor : zone.wrongColor;  // Usamos GetZoneRenderer()

            if (!zoneCorrect)
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            yield return new WaitForSeconds(victoryDelay);
            ShowVictoryMessage();
        }
        else
        {
            // Si no todos los cubos están correctos, volver a intentar
            StartCoroutine(HandleIncorrectPlacement());
        }

        isVerificationCompleted = true; // Marcar que la verificación se ha completado
    }


    private IEnumerator HandleIncorrectPlacement()
    {
        yield return new WaitForSeconds(0.5f);

        // Usar un for en lugar de foreach para poder eliminar elementos de currentCubes
        for (int i = 0; i < safeZones.Count; i++)
        {
            SafeZone2 zone = safeZones[i];

            // Iterar sobre los cubos dentro de la zona usando un for
            for (int j = 0; j < zone.currentCubes.Count; j++)
            {
                CubeController2 cube = zone.currentCubes[j];

                // Si el cubo es incorrecto, liberarlo y restablecer su posición
                if (cube.cubeID != zone.zoneID)
                {
                    cube.Unlock();
                    cube.ResetPosition();
                }
            }

            zone.ResetZone(); // Resetear la zona después de verificar los cubos
        }
    }

    private void ShowVictoryMessage()
    {
        // Mostrar mensaje de victoria
        if (winMessagePanel != null)
        {
            winMessagePanel.SetActive(true);
        }

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "TutorialMinijuego4")
        {
            // Si gana el tutorial, cambiar a Minijuego4Level1
            SceneManager.LoadScene("Minijuego4Level1");
        }
        else if (currentScene == "Minijuego4Level1")
        {
            // Si gana el nivel, cambiar a Lobby
            SceneManager.LoadScene("Lobby");
        }

        Debug.Log("¡Victoria! Todos los cubos están correctamente colocados");
    }

    private void GameOver()
    {
        // Mostrar el panel de game over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true; // Marcar que el juego terminó
        Debug.Log("¡Game Over! El tiempo se agotó");
    }

    // Reiniciar el nivel actual
    private void RestartLevel()
    {
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargar la escena actual
        gameOverPanel.SetActive(false); // Ocultar panel de game over
    }
}
