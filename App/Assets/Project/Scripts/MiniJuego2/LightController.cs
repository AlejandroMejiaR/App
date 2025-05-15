using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light[] lights = new Light[3];
    
    [Range(5f, 10f)]
    public float intensityStep = 5f;
    
    private void Start()
    {
        // Initialize all lights with intensity 0
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i] != null)
            {
                lights[i].intensity = 0f;
            }
        }
    }
    
    public void SetIntensity(int lightIndex, float value)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null) return;
        
        lights[lightIndex].intensity = value;
    }
    
    public void ChangeColor(int lightIndex, Color newColor)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null) return;
        
        lights[lightIndex].color = newColor;
    }
    
    public bool IsLightInTargetRange(int lightIndex)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null || 
            GameManager.Instance == null || GameManager.Instance.currentLevel == null || 
            GameManager.Instance.currentLevel.targetLightSettings.Length <= lightIndex) 
            return false;
        
        var targetSettings = GameManager.Instance.currentLevel.targetLightSettings[lightIndex];
        
        float intensity = lights[lightIndex].intensity;
        bool intensityInRange = intensity >= targetSettings.minIntensity && 
                                intensity <= targetSettings.maxIntensity;
        
        bool colorMatches = ColorDifference(lights[lightIndex].color, targetSettings.targetColor) <= targetSettings.colorTolerance;
        
        return intensityInRange && colorMatches;
    }

    public bool IsIntensityInTargetRange(int lightIndex)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null ||
            GameManager.Instance == null || GameManager.Instance.currentLevel == null ||
            GameManager.Instance.currentLevel.targetLightSettings.Length <= lightIndex)
            return false;

        var targetSettings = GameManager.Instance.currentLevel.targetLightSettings[lightIndex];

        float intensity = lights[lightIndex].intensity;

        // Validar que la intensidad esté dentro del rango total permitido
        if (intensity < targetSettings.minIntensity || intensity > targetSettings.maxIntensity)
            return false;

        // Validar que la intensidad esté dentro del rango correcto
        bool intensityInCorrectRange = intensity >= targetSettings.correctMinIntensity && intensity <= targetSettings.correctMaxIntensity;

        return intensityInCorrectRange;
    }


    public bool IsColorInTargetRange(int lightIndex)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null || 
            GameManager.Instance == null || GameManager.Instance.currentLevel == null || 
            GameManager.Instance.currentLevel.targetLightSettings.Length <= lightIndex) 
            return false;
            
        var targetSettings = GameManager.Instance.currentLevel.targetLightSettings[lightIndex];
        float diff = ColorDifference(lights[lightIndex].color, targetSettings.targetColor);
        
        return diff <= targetSettings.colorTolerance;
    }
    
    private float ColorDifference(Color a, Color b)
    {
        float rDiff = a.r - b.r;
        float gDiff = a.g - b.g;
        float bDiff = a.b - b.b;
        
        return Mathf.Sqrt(rDiff*rDiff + gDiff*gDiff + bDiff*bDiff);
    }
}