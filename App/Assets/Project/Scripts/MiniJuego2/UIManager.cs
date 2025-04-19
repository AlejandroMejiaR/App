using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI levelCompleteText;
    public Button nextLevelButton;
    public Button restartButton;
    
    private CameraController cameraController;
    private LightController lightController;
    
    private void Start()
    {
        InitializeSliders();
        
        if (GameManager.Instance != null)
        {
            cameraController = GameManager.Instance.cameraController;
            lightController = GameManager.Instance.lightController;
            GameManager.Instance.uiManager = this;
        }

        if (nextCameraButton) nextCameraButton.onClick.AddListener(cameraController.NextCamera);
        if (prevCameraButton) prevCameraButton.onClick.AddListener(cameraController.PreviousCamera);
        
        if (captureButton) captureButton.onClick.AddListener(() => {
            GameManager.Instance.CaptureAndCheckWinCondition();
        });

        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] != null)
            {
                int index = i;
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
        
        if (restartButton) restartButton.onClick.AddListener(GameManager.Instance.RestartLevel);
        
        if (levelCompletePanel) levelCompletePanel.SetActive(false);
        if (cameraErrorIcon) cameraErrorIcon.SetActive(false);
        
        for (int i = 0; i < 3; i++)
        {
            if (intensityErrorIcons[i]) intensityErrorIcons[i].SetActive(false);
            if (colorErrorIcons[i]) colorErrorIcons[i].SetActive(false);
        }
        
        StartCoroutine(ForceSliderUpdate());
    }

    private void InitializeSliders()
    {
        for (int i = 0; i < intensitySliders.Length; i++)
        {
            if (intensitySliders[i] != null)
            {
                intensitySliders[i].minValue = 0;
                intensitySliders[i].maxValue = 10;
                intensitySliders[i].wholeNumbers = true;
                intensitySliders[i].value = 0;
                intensitySliders[i].SetValueWithoutNotify(0);

                int index = i;
                intensitySliders[i].onValueChanged.AddListener((float value) => {
                    if (lightController != null)
                    {
                        lightController.SetIntensity(index, value);
                        // Eliminada la llamada a UpdateLightLabel
                    }
                });
            }
        }
    }

    private System.Collections.IEnumerator ForceSliderUpdate()
    {
        yield return null;
        
        if (GameManager.Instance != null && GameManager.Instance.currentLevel != null)
        {
            for (int i = 0; i < intensitySliders.Length; i++)
            {
                if (i < GameManager.Instance.currentLevel.targetLightSettings.Length && 
                    intensitySliders[i] != null)
                {
                    float maxIntensity = Mathf.Max(8f, 
                        GameManager.Instance.currentLevel.targetLightSettings[i].maxIntensity + 0.5f);
                    intensitySliders[i].maxValue = maxIntensity;
                }
            }
        }

        for (int i = 0; i < intensitySliders.Length; i++)
        {
            if (intensitySliders[i] != null)
            {
                intensitySliders[i].value = 0;
                intensitySliders[i].SetValueWithoutNotify(0);
            }
        }
    }
    
    public void UpdateUI()
    {
        if (cameraText && cameraController) 
            cameraText.text = $"Camera {cameraController.CurrentCameraIndex + 1}";
        
        for (int i = 0; i < intensitySliders.Length; i++)
        {
            if (lightController && i < lightController.lights.Length && lightController.lights[i] != null)
            {
                if (intensitySliders[i])
                    intensitySliders[i].value = lightController.lights[i].intensity;
                
                if (colorButtons[i])
                    colorButtons[i].color = lightController.lights[i].color;
                
                // Eliminada la llamada a UpdateLightLabel
            }
        }
    }
    
    private void CycleColor(int lightIndex)
    {
        if (lightController == null || lightController.lights[lightIndex] == null) return;
        
        Color[] colorOptions = new Color[] 
        {
            Color.white,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.cyan,
            Color.magenta
        };
        
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
        
        int nextIndex = (closestIndex + 1) % colorOptions.Length;
        lightController.ChangeColor(lightIndex, colorOptions[nextIndex]);
        
        if (colorButtons[lightIndex] != null)
        {
            colorButtons[lightIndex].color = colorOptions[nextIndex];
        }
    }
    
    public void ShowCaptureResults(bool correctCamera, bool[] intensityCorrect, bool[] colorCorrect)
    {
        if (cameraErrorIcon) cameraErrorIcon.SetActive(!correctCamera);
        
        for (int i = 0; i < 3; i++)
        {
            if (i < intensityErrorIcons.Length && intensityErrorIcons[i] != null)
            {
                intensityErrorIcons[i].SetActive(!intensityCorrect[i]);
            }
            
            if (i < colorErrorIcons.Length && colorErrorIcons[i] != null)
            {
                colorErrorIcons[i].SetActive(!colorCorrect[i]);
            }
        }
    }
    
    public void ShowLevelCompleteUI()
    {
        if (levelCompletePanel) 
        {
            levelCompletePanel.SetActive(true);
            
            if (SceneManager.GetActiveScene().name == "TutorialMinijuego2")
            {
                levelCompleteText.text = "Level Complete!";
                nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
                nextLevelButton.onClick.RemoveAllListeners();
                nextLevelButton.onClick.AddListener(() => {
                    SceneManager.LoadScene("Minijuego2Level1");
                });
            }
            else // Para Minijuego2Level1
            {
                levelCompleteText.text = "Game Complete!";
                nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lobby";
                nextLevelButton.onClick.RemoveAllListeners();
                nextLevelButton.onClick.AddListener(() => {
                    SceneManager.LoadScene("Lobby");
                });
            }
        }
    }

}