using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public LevelData currentLevel;
    public CameraController cameraController;
    public LightController lightController;
    public UIManager uiManager;
    
    private bool isLevelComplete = false;
    
    private void Awake()
    {
        // Singleton pattern
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
    
    public void CaptureAndCheckWinCondition()
    {
        if (isLevelComplete) return;
    
        // Debug target settings
        Debug.Log("=== TARGET SETTINGS ===");
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"Light {i}: Intensity({currentLevel.targetLightSettings[i].minIntensity} - {currentLevel.targetLightSettings[i].maxIntensity}), " +
                    $"Color({currentLevel.targetLightSettings[i].targetColor.r}, {currentLevel.targetLightSettings[i].targetColor.g}, {currentLevel.targetLightSettings[i].targetColor.b}), " +
                    $"Tolerance: {currentLevel.targetLightSettings[i].colorTolerance}");
        }
        
        // Debug current settings
        Debug.Log("=== CURRENT SETTINGS ===");
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"Light {i}: Intensity({lightController.lights[i].intensity}), " +
                    $"Color({lightController.lights[i].color.r}, {lightController.lights[i].color.g}, {lightController.lights[i].color.b})");
        }
        
        // Check if correct camera is selected
        bool correctCamera = cameraController.CurrentCameraIndex == currentLevel.correctCameraIndex;
        
        // Check if all lights are within correct range
        bool allLightsCorrect = true;
        bool[] lightIntensityCorrect = new bool[3];
        bool[] lightColorCorrect = new bool[3];
        
        for (int i = 0; i < 3; i++)
        {
            // Get individual checks for feedback
            lightIntensityCorrect[i] = lightController.IsIntensityInTargetRange(i);
            lightColorCorrect[i] = lightController.IsColorInTargetRange(i);
            
            if (!lightIntensityCorrect[i] || !lightColorCorrect[i])
            {
                allLightsCorrect = false;
            }
        }
        
        // Update UI to show feedback
        uiManager.ShowCaptureResults(correctCamera, lightIntensityCorrect, lightColorCorrect);
        
        // Check for win
        if (correctCamera && allLightsCorrect)
        {
            LevelComplete();
        }
    }
    
    private void LevelComplete()
    {
        isLevelComplete = true;
        Debug.Log("Level Complete!");
        
        // Show level complete UI if UIManager is available
        if (uiManager != null)
        {
            uiManager.ShowLevelCompleteUI();
        }
    }
    
    public void LoadNextLevel()
    {
        // Load next level logic
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Game complete
            if (uiManager != null)
            {
                uiManager.ShowGameCompleteUI();
            }
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}