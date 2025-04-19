using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Camera Controls")]
    public Button nextCameraButton;
    public Button prevCameraButton;
    public TextMeshProUGUI cameraText;
    public GameObject cameraErrorIcon;
    
    [Header("Light Controls")]
    public Slider[] intensitySliders = new Slider[3];
    public Image[] colorButtons = new Image[3];
    public TextMeshProUGUI[] lightLabels = new TextMeshProUGUI[3];
    public GameObject[] intensityErrorIcons = new GameObject[3];
    public GameObject[] colorErrorIcons = new GameObject[3];
    
    [Header("Game UI")]
    public Button captureButton;
    public GameObject levelCompletePanel;
    public GameObject gameCompletePanel;
    public Button nextLevelButton;
    public Button restartButton;
    
    private CameraController cameraController;
    private LightController lightController;
    
    private void Start()
    {
        // Get references from GameManager
        if (GameManager.Instance != null)
        {
            cameraController = GameManager.Instance.cameraController;
            lightController = GameManager.Instance.lightController;
            
            // Update reference to UIManager in GameManager
            GameManager.Instance.uiManager = this;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }
        
        // Set up camera buttons
        if (nextCameraButton) nextCameraButton.onClick.AddListener(cameraController.NextCamera);
        if (prevCameraButton) prevCameraButton.onClick.AddListener(cameraController.PreviousCamera);
        
        // Set up capture button
        if (captureButton) captureButton.onClick.AddListener(() => {
            GameManager.Instance.CaptureAndCheckWinCondition();
        });
        
        // Initialize sliders with proper min/max values and starting at 0
        for (int i = 0; i < intensitySliders.Length; i++)
        {
            if (intensitySliders[i] != null && GameManager.Instance.currentLevel != null)
            {
                // Set slider min to 0
                intensitySliders[i].minValue = 0;
                
                // Set slider max to match the max intensity from level data
                // Add a small buffer to allow setting slightly above if needed
                float maxIntensity = GameManager.Instance.currentLevel.targetLightSettings[i].maxIntensity + 1f;
                intensitySliders[i].maxValue = maxIntensity;
                
                // Set initial value to 0
                intensitySliders[i].value = 0;
            }
        }
        
        // Set up color picker buttons (simple color toggle)
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] != null)
            {
                int index = i; // Capture for lambda
                Button btn = colorButtons[i].GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.AddListener(() => 
                    {
                        CycleColor(index);
                    });
                }
            }
        }
        
        // Set up game UI buttons
        if (nextLevelButton) nextLevelButton.onClick.AddListener(GameManager.Instance.LoadNextLevel);
        if (restartButton) restartButton.onClick.AddListener(GameManager.Instance.RestartLevel);
        
        // Hide completion panels initially
        if (levelCompletePanel) levelCompletePanel.SetActive(false);
        if (gameCompletePanel) gameCompletePanel.SetActive(false);
        
        // Hide all error icons initially
        if (cameraErrorIcon) cameraErrorIcon.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            if (intensityErrorIcons[i]) intensityErrorIcons[i].SetActive(false);
            if (colorErrorIcons[i]) colorErrorIcons[i].SetActive(false);
        }
        
        // Update UI to match initial state
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        // Update camera text
        if (cameraText && cameraController) 
            cameraText.text = $"Camera {cameraController.CurrentCameraIndex + 1}";
        
        // Update light sliders and colors
        for (int i = 0; i < intensitySliders.Length; i++)
        {
            if (lightController && i < lightController.lights.Length && lightController.lights[i] != null)
            {
                if (intensitySliders[i])
                    intensitySliders[i].value = lightController.lights[i].intensity;
                
                if (colorButtons[i])
                    colorButtons[i].color = lightController.lights[i].color;
                
                UpdateLightLabel(i);
            }
        }
    }
    
    private void UpdateLightLabel(int index)
    {
        if (lightLabels[index] && lightController && index < lightController.lights.Length && lightController.lights[index] != null)
        {
            lightLabels[index].text = $"Light {index+1}: {lightController.lights[index].intensity:F1}";
        }
    }
    
    private void CycleColor(int lightIndex)
    {
        if (lightController == null || lightController.lights[lightIndex] == null) return;
        
        // Simple color cycling through predefined colors
        Color[] colorOptions = new Color[] 
        {
            Color.white,     // (1,1,1)
            Color.red,       // (1,0,0)
            Color.green,     // (0,1,0)
            Color.blue,      // (0,0,1)
            Color.yellow,    // (1,1,0)
            Color.cyan,      // (0,1,1)
            Color.magenta    // (1,0,1)
        };
        
        // Find the closest current color
        Color currentColor = lightController.lights[lightIndex].color;
        int closestIndex = 0;
        float minDistance = float.MaxValue;
        
        for (int i = 0; i < colorOptions.Length; i++)
        {
            float distance = Vector4.Distance(currentColor, colorOptions[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }
        
        // Move to the next color
        int nextIndex = (closestIndex + 1) % colorOptions.Length;
        
        // Apply the color to both the light and the UI button
        lightController.lights[lightIndex].color = colorOptions[nextIndex];
        
        if (colorButtons[lightIndex] != null)
        {
            colorButtons[lightIndex].color = colorOptions[nextIndex];
        }
        
        // Log for debugging
        Debug.Log($"Changed light {lightIndex} color to: {colorOptions[nextIndex]}");
    }
    
    public void ShowCaptureResults(bool correctCamera, bool[] intensityCorrect, bool[] colorCorrect)
    {
        // Show camera error icon if needed
        if (cameraErrorIcon) cameraErrorIcon.SetActive(!correctCamera);
        
        // Show intensity and color error icons if needed
        for (int i = 0; i < 3; i++)
        {
            if (intensityErrorIcons[i]) intensityErrorIcons[i].SetActive(!intensityCorrect[i]);
            if (colorErrorIcons[i]) colorErrorIcons[i].SetActive(!colorCorrect[i]);
        }
    }
    
    public void ShowLevelCompleteUI()
    {
        if (levelCompletePanel) 
        {
            levelCompletePanel.SetActive(true);
        }
    }
    
    public void ShowGameCompleteUI()
    {
        if (gameCompletePanel) 
        {
            gameCompletePanel.SetActive(true);
        }
    }
}