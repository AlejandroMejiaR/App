using UnityEngine;
using System.Collections;
using General;

public class triggertp : MonoBehaviour
{

    [SerializeField] Transform newPos;
    [SerializeField] Transform newPosCam;
    private GameObject cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Tp.Instance.newTp(other.gameObject,newPos);
            Tp.Instance.newTp(cam,newPosCam);



        }
    }
    private void Start()
    {
        cam = Camera.main.gameObject;
    }
}
