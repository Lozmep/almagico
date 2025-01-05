using UnityEngine;

public class TakeTinto : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactionDistance = 2f;  // Distancia para interactuar con el cubo.
    public KeyCode interactKey = KeyCode.X;  // Tecla para interactuar.
    private GameObject currentObject;       // Referencia al cubo que el jugador está tomando.

    [Header("Posición mientras lo cargas")]
    public Vector3 holdPositionOffset = new Vector3(0, -0.5f, 1);  // Offset para posicionar el cubo en el jugador.

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (currentObject == null)
            {
                GrabObject();  // Agarrar el cubo si el jugador no lo está tomando.
            }
        }
    }

    private void GrabObject()
    {
        // Buscar objetos cercanos dentro de la distancia especificada.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("CoffeeCube"))
            {
                currentObject = hitCollider.gameObject;
                GrabItem();
                return;  // Sale del bucle después de agarrar el objeto.
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

            // Posicionar el cubo en el `holdPositionOffset` del jugador.
            currentObject.transform.SetParent(transform);
            currentObject.transform.localPosition = holdPositionOffset;
            currentObject.transform.localRotation = Quaternion.identity;
        }
    }
}