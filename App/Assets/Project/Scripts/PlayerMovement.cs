using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad del jugador
    public float rotationSpeed = 10f; // Velocidad de rotación
    private Rigidbody rb;
    private Vector3 moveDirection;

    public Joystick joystick; // Referencia al joystick virtual
    public GameObject joystickUI; // Panel del joystick para ocultarlo en PC

    private bool isMobile; // Detectar si está en móvil

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el Rigidbody rote por colisiones
        rb.linearDamping = 25f; // Controla la fricción y evita deslizamiento

        // Detectar si el juego está corriendo en un dispositivo móvil
        #if UNITY_IOS || UNITY_ANDROID
            isMobile = true;
        #else
            isMobile = false;
        #endif

        // Ocultar el joystick en PC
        if (joystickUI != null)
        {
            joystickUI.SetActive(isMobile);
        }
    }

    void Update()
    {
        // Leer la entrada del teclado o joystick virtual
        float moveX = isMobile ? joystick.Horizontal : Input.GetAxis("Horizontal");
        float moveZ = isMobile ? joystick.Vertical : Input.GetAxis("Vertical");

        // Convertir entrada en movimiento en el mundo isométrico
        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = Quaternion.Euler(0, 45, 0) * moveDirection; // Rotar para alinearlo con la vista isométrica

        // Si hay movimiento, rotar el personaje
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
            // Aplicar movimiento normal
            rb.linearVelocity = moveDirection.normalized * speed + new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            // Detención instantánea
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; // Evita cualquier giro involuntario
        }
    }
}
