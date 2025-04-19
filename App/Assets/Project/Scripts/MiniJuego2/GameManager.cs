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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void CaptureAndCheckWinCondition()
    {
        if (isLevelComplete) return;
    
        bool correctCamera = cameraController.CurrentCameraIndex == currentLevel.correctCameraIndex;
        bool allLightsCorrect = true;
        bool[] lightIntensityCorrect = new bool[3];
        bool[] lightColorCorrect = new bool[3];
        
        for (int i = 0; i < 3; i++)
        {
            lightIntensityCorrect[i] = lightController.IsIntensityInTargetRange(i);
            lightColorCorrect[i] = lightController.IsColorInTargetRange(i);
            
            if (!lightIntensityCorrect[i] || !lightColorCorrect[i])
            {
                allLightsCorrect = false;
            }
        }
        
        if (uiManager != null)
        {
            uiManager.ShowCaptureResults(correctCamera, lightIntensityCorrect, lightColorCorrect);
        }
        
        if (correctCamera && allLightsCorrect)
        {
            LevelComplete();
        }
    }   
    
    private void LevelComplete()
    {
        isLevelComplete = true;
        
        if (uiManager != null)
        {
            uiManager.ShowLevelCompleteUI();
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}