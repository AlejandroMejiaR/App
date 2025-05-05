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

            bool hasIncorrectCube = false;
            foreach (CubeController2 cube in zone.currentCubes)
            {
                if (cube.cubeID != zone.zoneID)
                {
                    hasIncorrectCube = true;
                    allCorrect = false;
                    break;
                }
            }
            
            zone.GetZoneRenderer().material.color = hasIncorrectCube ? zone.wrongColor : zone.correctColor;
        }

        if (!allZonesOccupied || !allCorrect)
        {
            yield return new WaitForSeconds(0.5f);
            
            // Segunda pasada: Eliminar solo los cubos incorrectos
            foreach (SafeZone2 zone in safeZones)
            {
                List<CubeController2> cubesToRemove = new List<CubeController2>();
                
                // Identificar cubos incorrectos
                foreach (CubeController2 cube in zone.currentCubes)
                {
                    if (cube.cubeID != zone.zoneID)
                    {
                        cubesToRemove.Add(cube);
                    }
                }

                // Eliminar solo los cubos incorrectos
                foreach (CubeController2 cube in cubesToRemove)
                {
                    cube.Unlock();
                    cube.ResetPosition();
                    zone.ReleaseCube(cube);
                }

                // Actualizar color de la zona
                if (zone.currentCubes.Count > 0)
                {
                    bool isCorrect = true;
                    foreach (CubeController2 cube in zone.currentCubes)
                    {
                        if (cube.cubeID != zone.zoneID)
                        {
                            isCorrect = false;
                            break;
                        }
                    }
                    zone.GetZoneRenderer().material.color = isCorrect ? zone.correctColor : zone.wrongColor;
                }
                else
                {
                    zone.GetZoneRenderer().material.color = zone.neutralColor;
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
            ConfigureVictoryButton();
            
            // Buscar el botón de continuar en el panel de victoria
            Button victoryContinueButton = winMessagePanel.GetComponentInChildren<Button>();
            if (victoryContinueButton != null)
            {
                // Limpiar listeners previos y asignar el nuevo
                victoryContinueButton.onClick.RemoveAllListeners();
                
                string currentScene = SceneManager.GetActiveScene().name;
                if (currentScene == "TutorialMinijuego4")
                {
                    // Tutorial sigue siendo automático
                    victoryContinueButton.onClick.AddListener(() => SceneManager.LoadScene("Minijuego4Level1"));
                }
                else if (currentScene == "Minijuego4Level1")
                {
                    // Nivel 1 requiere click para continuar
                    victoryContinueButton.onClick.AddListener(() => SceneManager.LoadScene("Lobby"));
                }
            }
        }

        // Eliminar la carga automática de escena que estaba aquí
        Debug.Log("¡Victoria! Todos los cubos están correctamente colocados");
    }

        private void ConfigureVictoryButton()
    {
        if (winMessagePanel == null) return;
        
        Button victoryButton = winMessagePanel.GetComponentInChildren<Button>();
        if (victoryButton == null) return;
        
        victoryButton.onClick.RemoveAllListeners();
        
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "TutorialMinijuego4")
        {
            victoryButton.onClick.AddListener(() => SceneManager.LoadScene("Minijuego4Level1"));
        }
        else if (currentScene == "Minijuego4Level1")
        {
            victoryButton.onClick.AddListener(() => SceneManager.LoadScene("Lobby"));
        }
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
