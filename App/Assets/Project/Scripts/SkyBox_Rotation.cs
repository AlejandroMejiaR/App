using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Grados por segundo

    void Update()
    {
        // Obtiene la rotación actual
        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");

        // Calcula la nueva rotación
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;

        // Aplica la rotación al skybox
        RenderSettings.skybox.SetFloat("_Rotation", newRotation);
    }
}

