using UnityEngine;

public class DialogNPC : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactionDistance = 2f;  // Distancia para interactuar con el cubo.
    public KeyCode interactKey = KeyCode.X;  // Tecla para interactuar.
    private GameObject currentObject;       // Referencia al objeto que el jugador está tomando.
    public GameObject otherCharacter;        // Referencia al otro personaje para interactuar.
    
    [Header("Posición mientras lo cargas")]
    public Vector3 holdPositionOffset = new Vector3(0, -0.5f, 1);  // Offset para posicionar el cubo.

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (currentObject == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.CompareTag("CoffeeCube"))
                    {
                        currentObject = hitCollider.gameObject;
                        GrabObject();
                        break;
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        CheckProximityToOtherCharacter();
    }

    private void GrabObject()
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
        }
    }

    private void DropObject()
    {
        if (currentObject != null)
        {
            Collider cubeCollider = currentObject.GetComponent<Collider>();
            Rigidbody cubeRb = currentObject.GetComponent<Rigidbody>();

            if (cubeCollider != null)
                cubeCollider.enabled = true;

            if (cubeRb != null)
                cubeRb.isKinematic = false;

            currentObject.transform.SetParent(null);
            currentObject = null;
        }
    }

    private void CheckProximityToOtherCharacter()
    {
        if (currentObject != null && otherCharacter != null)
        {
            float distance = Vector3.Distance(transform.position, otherCharacter.transform.position);

            if (distance < interactionDistance)
            {
                Debug.Log("El otro personaje despliega el diálogo: '¡Hola! Gracias por traerme el cubo.'");
            }
        }
    }
}
