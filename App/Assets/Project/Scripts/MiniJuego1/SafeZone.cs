using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Configuración")]
    public int zoneID;
    public float detectionRadius = 1f;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color neutralColor = Color.gray;
    
    [Header("Referencias")]
    [SerializeField] private Renderer zoneRenderer;
    public CubeController currentCube { get; private set; }

    private void Start()
    {
        if(zoneRenderer == null)
            zoneRenderer = GetComponent<Renderer>();
        ResetZone();
    }

    public void CheckForCube(CubeController cube)
    {
        if(cube.isLocked) return;
        
        float distance = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(cube.transform.position.x, 0, cube.transform.position.z)
        );

        if(distance <= detectionRadius && cube.transform.position.y <= 0.5f)
        {
            currentCube = cube;
            cube.currentSafeZone = this;
            cube.isInSafeZone = true;
            cube.transform.position = new Vector3(
                transform.position.x,
                0f,
                transform.position.z
            );
        }
    }

    public bool IsCorrectCube()
    {
        if(currentCube == null) return false;
        
        bool isCorrect = (currentCube.cubeID == zoneID);
        zoneRenderer.material.color = isCorrect ? correctColor : wrongColor;
        return isCorrect;
    }

    public void ReleaseCube(CubeController cube)
    {
        if(currentCube == cube)
        {
            currentCube = null;
            ResetZone();
        }
    }

    public void ResetZone()
    {
        zoneRenderer.material.color = neutralColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}