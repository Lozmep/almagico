using EventManager;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidad del personaje
    private Rigidbody rb;

    private Vector3 moveDirection;

    [Header("Wall Detector Settings")]
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public float verticalDistance = 0.9f;
    public Color rayColorHit = Color.red;
    public Color rayColorMiss = Color.green;

    

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Asegurar que la gravedad funcione correctamente
        rb.freezeRotation = true;
    }

    void Update()
    {
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask)) return;
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

    private void OnDrawGizmos()
    {
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);

        // Comprueba si el raycast detectaría algo
        bool hitSomething = Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask);

        // Cambia el color del Gizmo dependiendo del resultado del raycast
        Gizmos.color = hitSomething ? rayColorHit : rayColorMiss;

        // Dibuja la línea representando el raycast
        Gizmos.DrawLine(origin, hitSomething ? hit.point : origin + direction * maxDistance);

        // Si hay un impacto, dibuja una pequeña esfera en el punto de impacto
        if (hitSomething)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }

}