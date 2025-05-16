using UnityEngine;
using System.Collections;
using General;

public class triggertp : MonoBehaviour
{
    [SerializeField] Transform newPos;
    [SerializeField] Transform newPosCam;
    [SerializeField] int cameraIndex;  // Índice para guardar

    private GameObject cam;

    private void Start()
    {
        cam = Camera.main.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Tp.Instance.newTp(other.gameObject, newPos);
            Tp.Instance.newTp(cam, newPosCam);

            // Guardar índice cámara en player state
            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.SaveCameraPositionIndex(cameraIndex);
            }
        }
    }
}
