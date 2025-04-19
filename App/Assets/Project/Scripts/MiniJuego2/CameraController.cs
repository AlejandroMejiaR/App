using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras = new Camera[3];
    private int currentCameraIndex = 0;
    
    public int CurrentCameraIndex => currentCameraIndex;
    
    private void Start()
    {
        // Disable all cameras except the first one
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }
    
    public void SwitchCamera(int index)
    {
        if (index < 0 || index >= cameras.Length) return;
        
        // Disable current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);
        
        // Enable new camera
        currentCameraIndex = index;
        cameras[currentCameraIndex].gameObject.SetActive(true);
        
        Debug.Log($"Switched to camera {currentCameraIndex}");
    }
    
    public void NextCamera()
    {
        SwitchCamera((currentCameraIndex + 1) % cameras.Length);
    }
    
    public void PreviousCamera()
    {
        SwitchCamera((currentCameraIndex - 1 + cameras.Length) % cameras.Length);
    }
}