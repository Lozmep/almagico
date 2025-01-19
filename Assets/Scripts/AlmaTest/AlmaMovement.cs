using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlmaMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float velocidadMoviento = 40.0f;
    public float velocidadRotacion = 200.0f;

    private Animator anim;

    public float x, y;

    [Header("Wall Detector Settings")]
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    private Vector3 reverseDirection;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public float verticalDistance = 1f;
    public Color rayColorHit = Color.red;
    public Color rayColorMiss = Color.green;
    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueManager.isActive)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = transform.forward + new Vector3(0, -verticalDistance, 0);
        Vector3 reverseDirection = -transform.forward + new Vector3(0, -verticalDistance, 0);
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);


        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
        {
            y = Input.GetAxis("Vertical");

            if (y > 0)
            {
                y = 0;
            }

        }
        else if (Physics.Raycast(origin, reverseDirection, maxDistance, layerMask))
        {
            y = Input.GetAxis("Vertical");

            if (y < 0)
            {
                y = 0;
            }
        }
        else
        {
            y = Input.GetAxis("Vertical");
        }

        x = Input.GetAxis("Horizontal");

        transform.Translate(0, 0, y * Time.deltaTime * velocidadMoviento);
        transform.Rotate(0, x * Time.deltaTime * velocidadRotacion, 0);


    }

    private void OnDrawGizmos()
    {
        Vector3 direction = -transform.forward + new Vector3(0, -verticalDistance, 0);
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);

        // Comprueba si el raycast detectara algo
        bool hitSomething = Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask);

        // Cambia el color del Gizmo dependiendo del resultado del raycast
        Gizmos.color = hitSomething ? rayColorHit : rayColorMiss;

        // Dibuja la linea representando el raycast
        Gizmos.DrawLine(origin, hitSomething ? hit.point : origin + direction * maxDistance);

        // Si hay un impacto, dibuja una pequena esfera en el punto de impacto
        if (hitSomething)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
