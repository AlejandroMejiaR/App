using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeGameManager : MonoBehaviour
{
    public static CubeGameManager Instance;

    [Header("Referencias")]
    public List<CubeController> allCubes = new List<CubeController>();
    public List<SafeZone> safeZones = new List<SafeZone>();
    public GameObject winMessagePanel; // Referencia al panel de victoria

    [Header("Configuración")]
    public float verificationDelay = 1f;
    public float victoryAnimationHeight = 2f;
    public float zoneLowerHeight = -1f;
    public float animationDuration = 1f;
    public float victoryDelay = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (winMessagePanel != null)
            winMessagePanel.SetActive(false); // Asegurar que está oculto al inicio
    }

    public void CheckAllCubes()
    {
        bool allPlaced = true;
        foreach (CubeController cube in allCubes)
        {
            if (!cube.isInSafeZone)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            StartCoroutine(VerifyCubesCoroutine());
        }
    }

    private IEnumerator VerifyCubesCoroutine()
    {
        yield return new WaitForSeconds(verificationDelay);

        bool allCorrect = true;
        foreach (SafeZone zone in safeZones)
        {
            if (!zone.IsCorrectCube())
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            yield return new WaitForSeconds(victoryDelay);
            StartCoroutine(VictoryAnimation());
        }
        else
        {
            StartCoroutine(HandleIncorrectPlacement());
        }
    }

    private IEnumerator HandleIncorrectPlacement()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (SafeZone zone in safeZones)
        {
            if (zone.currentCube != null && !zone.IsCorrectCube())
            {
                zone.currentCube.Unlock();
                zone.currentCube.ResetPosition();
                zone.ResetZone();
            }
        }
    }

    private IEnumerator VictoryAnimation()
    {
        // Animación de cubos
        foreach (CubeController cube in allCubes)
        {
            cube.MoveToHeight(victoryAnimationHeight);
        }

        // Animación de zonas
        foreach (SafeZone zone in safeZones)
        {
            StartCoroutine(zone.MoveDown(zoneLowerHeight, animationDuration));
        }

        yield return new WaitForSeconds(1f);

        // Mostrar mensaje de victoria
        if (winMessagePanel != null)
        {
            winMessagePanel.SetActive(true);
        }

        Debug.Log("¡Victoria! Todos los cubos están correctamente colocados");
    }
}
