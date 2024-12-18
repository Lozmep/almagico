using UnityEngine;

public class GiveTinto : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactionDistance = 2f;  // Distancia para interactuar con el cubo.
    public KeyCode interactKey = KeyCode.X;  // Tecla para interactuar.
    private GameObject currentObject;       // Referencia al objeto que el jugador está tomando.
    public GameObject otherCharacter;        // Referencia al otro personaje para interactuar.

    [Header("Posición mientras lo cargas")]
    public Vector3 holdPositionOffset = new Vector3(0, -0.5f, 1);  // Offset para posicionar el cubo en tu jugador.

    [Header("Posición en el otro personaje")]
    [SerializeField]
    private Vector3 holdPositionOffsetOther = new Vector3(0, 0.5f, 1);  // Offset editable desde el Inspector para el otro personaje.

    private bool isNearOtherCharacter = false;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (currentObject == null)
            {
                GrabObject();
            }
            else if (isNearOtherCharacter)
            {
                PassObjectToOtherCharacter();
            }
        }

        CheckProximityToOtherCharacter();
    }

    private void GrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("CoffeeCube"))
            {
                currentObject = hitCollider.gameObject;
                GrabItem();
                break;
            }
        }
    }

    private void GrabItem()
    {
        if (currentObject != null)
        {
            Collider cubeCollider = currentObject.GetComponent<Collider>();
            Rigidbody cubeRb = currentObject.GetComponent<Rigidbody>();

            if (cubeCollider != null)
                cubeCollider.enabled = false;

            if (cubeRb != null)
                cubeRb.isKinematic = true;

            currentObject.transform.SetParent(transform);
            currentObject.transform.localPosition = holdPositionOffset;
            currentObject.transform.localRotation = Quaternion.identity;

            Debug.Log("Obtuviste Tinto.");
        }
    }

    private void CheckProximityToOtherCharacter()
    {
        float distance = Vector3.Distance(transform.position, otherCharacter.transform.position);
        isNearOtherCharacter = distance < interactionDistance;

        if (isNearOtherCharacter && currentObject != null)
        {
            Debug.Log("¿Hola qué tal, me trajiste mi tinto?.");
        }
    }

    private void PassObjectToOtherCharacter()
    {
        if (currentObject != null)
        {
            Debug.Log("Acabas de darle al NPC Tinto");

            // Asignar el cubo como hijo del otro personaje.
            currentObject.transform.SetParent(otherCharacter.transform);

            // Posicionar el cubo en el offset específico del otro personaje.
            currentObject.transform.localPosition = holdPositionOffsetOther;
            currentObject.transform.localRotation = Quaternion.identity;

            Rigidbody cubeRb = currentObject.GetComponent<Rigidbody>();
            Collider cubeCollider = currentObject.GetComponent<Collider>();

            if (cubeRb != null)
                cubeRb.isKinematic = true;

            if (cubeCollider != null)
                cubeCollider.enabled = false;

            Debug.Log("¡Muchas Gracias por el tinto!");

            currentObject = null;
        }
    }
}
