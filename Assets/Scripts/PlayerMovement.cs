using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidad del personaje
    private Rigidbody rb;

    private Vector3 moveDirection;

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Asegurar que la gravedad funcione correctamente
        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        // Capturar el input del jugador
        float horizontalInput = Input.GetAxis("Horizontal"); // A y D
        float verticalInput = Input.GetAxis("Vertical");     // W y S

        // Calcular dirección de movimiento
        moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
    }

    void HandleMovement()
    {
        // Mover el Rigidbody
        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}