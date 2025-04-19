using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light[] lights = new Light[3];
    
    [Range(0.1f, 10f)]
    public float intensityStep = 0.1f;
    
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
        
        lights[lightIndex].intensity = Mathf.Clamp(value, 0, 8);
        Debug.Log($"Light {lightIndex} intensity set to: {lights[lightIndex].intensity}");
    }
    
    public void ChangeColor(int lightIndex, Color newColor)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null) return;
        
        lights[lightIndex].color = newColor;
        Debug.Log($"Light {lightIndex} color changed to: {newColor}");
    }
    
    public bool IsLightInTargetRange(int lightIndex)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null || 
            GameManager.Instance == null || GameManager.Instance.currentLevel == null || 
            GameManager.Instance.currentLevel.targetLightSettings.Length <= lightIndex) 
            return false;
        
        var targetSettings = GameManager.Instance.currentLevel.targetLightSettings[lightIndex];
        
        // Check intensity range
        bool intensityInRange = lights[lightIndex].intensity >= targetSettings.minIntensity && 
                                lights[lightIndex].intensity <= targetSettings.maxIntensity;
        
        // Check color tolerance
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
        return lights[lightIndex].intensity >= targetSettings.minIntensity && 
                lights[lightIndex].intensity <= targetSettings.maxIntensity;
    }
    
    public bool IsColorInTargetRange(int lightIndex)
    {
        if (lightIndex < 0 || lightIndex >= lights.Length || lights[lightIndex] == null || 
            GameManager.Instance == null || GameManager.Instance.currentLevel == null || 
            GameManager.Instance.currentLevel.targetLightSettings.Length <= lightIndex) 
            return false;
            
        var targetSettings = GameManager.Instance.currentLevel.targetLightSettings[lightIndex];
        return ColorDifference(lights[lightIndex].color, targetSettings.targetColor) <= targetSettings.colorTolerance;
    }
    
    private float ColorDifference(Color a, Color b)
    {
        // Calculate Euclidean distance in RGB space (more precise)
        float rDiff = a.r - b.r;
        float gDiff = a.g - b.g;
        float bDiff = a.b - b.b;
        
        // Add some debug information
        Debug.Log($"Color comparison: Light({a.r},{a.g},{a.b}) Target({b.r},{b.g},{b.b}) Diff: {Mathf.Sqrt(rDiff*rDiff + gDiff*gDiff + bDiff*bDiff)}");
        
        return Mathf.Sqrt(rDiff*rDiff + gDiff*gDiff + bDiff*bDiff);
    }
}