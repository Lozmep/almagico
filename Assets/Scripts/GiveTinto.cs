using UnityEngine;

public class GiveTinto : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactionDistance = 2f;  // Distancia para interactuar con el cubo.
    public KeyCode interactKey = KeyCode.X;  // Tecla para interactuar.
    public GameObject otherCharacter;       // Referencia al otro personaje (NPC).

    private GameObject currentObject;        // Referencia al cubo que el jugador está cargando.

    [Header("Posición mientras lo cargas")]
    public Vector3 holdPositionOffset = new Vector3(0, -0.5f, 1);

    [Header("Posición en el otro personaje")]
    public Vector3 holdPositionOffsetOther = new Vector3(0, 0.5f, 1);

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (currentObject == null)
            {
                // Intentar agarrar el cubo si está cerca.
                GrabObject();
            }
            else if (IsNearOtherCharacter())
            {
                // Entregar el cubo al otro personaje.
                PassObjectToOtherCharacter();
            }
        }
    }

    private void GrabObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("CoffeeCube"))
            {
                currentObject = hitCollider.gameObject;

                // Asignar el cubo como hijo del jugador y posicionarlo.
                currentObject.transform.SetParent(transform);
                currentObject.transform.localPosition = holdPositionOffset;
                currentObject.transform.localRotation = Quaternion.identity;

                // Desactivar el collider y establecer Rigidbody en kinematic.
                var cubeCollider = currentObject.GetComponent<Collider>();
                var cubeRb = currentObject.GetComponent<Rigidbody>();
                if (cubeCollider) cubeCollider.enabled = false;
                if (cubeRb) cubeRb.isKinematic = true;

                return;
            }
        }
    }

    private bool IsNearOtherCharacter()
    {
        return Vector3.Distance(transform.position, otherCharacter.transform.position) < interactionDistance;
    }

    private void PassObjectToOtherCharacter()
    {
        if (currentObject != null)
        {
            // Colocar el cubo como hijo del otro personaje.
            currentObject.transform.SetParent(otherCharacter.transform);
            currentObject.transform.localPosition = holdPositionOffsetOther;
            currentObject.transform.localRotation = Quaternion.identity;

            var cubeCollider = currentObject.GetComponent<Collider>();
            var cubeRb = currentObject.GetComponent<Rigidbody>();
            
            if (cubeRb) cubeRb.isKinematic = true;
            if (cubeCollider) cubeCollider.enabled = false;

            currentObject = null;  // Limpiar la referencia después de entregar.
        }
    }
}
