using UnityEngine;
using System.Runtime.InteropServices; // Necesario para importar JavaScript en WebGL

public class PlayerMovement : MonoBehaviour
{
    public float pcSpeed = 5f;
    public float mobileSpeed = 5f; 
    public float pcRotationSpeed = 10f;
    public float mobileRotationSpeed = 25f; 

    private float speed;
    private float rotationSpeed;

    private Rigidbody rb;
    private Vector3 moveDirection;

    public Joystick joystick; 
    public GameObject joystickUI;

    private bool isMobile = false;

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool IsMobile();
    #endif

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.linearDamping = 25f;

        // Detectar si el juego está en móvil (WebGL o Android/iOS)
        #if UNITY_WEBGL && !UNITY_EDITOR
        isMobile = IsMobile();
        #else
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isMobile = true;
        }
        #endif

        // Ajustar velocidad y rotación según la plataforma
        if (isMobile)
        {
            speed = mobileSpeed;
            rotationSpeed = mobileRotationSpeed;
        }
        else
        {
            speed = pcSpeed;
            rotationSpeed = pcRotationSpeed;
        }

        // Mostrar joystick en móviles
        if (joystickUI != null)
        {
            joystickUI.SetActive(isMobile);
        }
    }

    void Update()
    {
        // Leer entrada del teclado o joystick
        float moveX = (isMobile ? joystick.Horizontal : Input.GetAxis("Horizontal"));
        float moveZ = (isMobile ? joystick.Vertical : Input.GetAxis("Vertical"));

        // Convertir entrada en movimiento isométrico
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = Quaternion.Euler(0, 45, 0) * moveDirection;

        // Rotación del personaje
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            rb.linearVelocity = moveDirection.normalized * speed + new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
