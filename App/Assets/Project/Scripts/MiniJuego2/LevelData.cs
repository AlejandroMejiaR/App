using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Minigame/Level Data")]
public class LevelData : ScriptableObject
{
    public int correctCameraIndex;
    
    [System.Serializable]
    public class LightSettings
    {
        public float minIntensity;
        public float maxIntensity;
        public Color targetColor;
        public float colorTolerance = 0.1f;
    }
    
    public LightSettings[] targetLightSettings = new LightSettings[3];
}