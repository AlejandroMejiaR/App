using UnityEngine;
using System.Collections.Generic;

public class SafeZone2 : MonoBehaviour
{
    [Header("Configuración")]
    public int zoneID;
    public float detectionRadius = 1f;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color neutralColor = Color.gray;

    [Header("Referencias")]
    [SerializeField] private Renderer zoneRenderer;  // Renderer privado para la zona

    public List<CubeController2> currentCubes = new List<CubeController2>();
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        if (zoneRenderer == null)
            zoneRenderer = GetComponent<Renderer>(); // Asignamos el Renderer si no está asignado
        ResetZone();
    }

    public Vector3 GetZoneDimensions()
    {
        return transform.localScale;
    }

    // Método getter para acceder al Renderer de la zona
    public Renderer GetZoneRenderer()
    {
        return zoneRenderer;
    }

    public void CheckForCube(CubeController2 cube)
    {
        if (cube.isLocked) return;

        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(cube.transform.position.x, 0, cube.transform.position.z)
        );

        if (distance <= detectionRadius && cube.transform.position.y <= 0.5f)
        {
            if (!currentCubes.Contains(cube))
            {
                currentCubes.Add(cube);
                cube.currentSafeZone = this;
                cube.isInSafeZone = true;
            }

            // Solo ajustamos la posición Y para que quede en el suelo
            cube.transform.position = new Vector3(
                cube.transform.position.x, // Mantener X original
                0f, // Forzar Y a 0
                cube.transform.position.z  // Mantener Z original
            );
        }
    }

    public bool IsCorrectCube()
    {
        if (currentCubes.Count == 0) return false;

        bool isCorrect = true;

        // Verificar que todos los cubos en la zona sean correctos
        foreach (var cube in currentCubes)
        {
            if (cube.cubeID != zoneID)
            {
                isCorrect = false;
                break; // Si encontramos un cubo incorrecto, no es una zona correcta
            }
        }

        // Cambiar el color de la zona en función de la validez de todos los cubos
        zoneRenderer.material.color = isCorrect ? correctColor : wrongColor;
        return isCorrect;
    }

    public void ReleaseCube(CubeController2 cube)
    {
        if (currentCubes.Contains(cube))
        {
            currentCubes.Remove(cube);
            cube.currentSafeZone = null;
            cube.isInSafeZone = false;
        }
    }

    public void ResetZone()
    {
        zoneRenderer.material.color = neutralColor;
        transform.position = initialPosition;

        foreach (var cube in currentCubes)
        {
            cube.isInSafeZone = false;
            cube.currentSafeZone = null;
        }
        currentCubes.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
