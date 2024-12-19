using UnityEngine;

public class TakeItem : MonoBehaviour
{
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;

    public GameObject tinto;
    public GameObject libro;

    public bool isFree;


    void Start()
    {
       isFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position;

        if (Input.GetKey(KeyCode.X) && isFree)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                
                isFree = false;
                string objectTag = hit.collider.tag;

                
                switch (objectTag)
                {
                    case "Tetera":
                        Debug.Log("Impacto con una Tetera: " + hit.collider.name);
                        tinto.SetActive(true);
                        break;

                    case "Librero":
                        Debug.Log("Impacto con un Cubo: " + hit.collider.name);
                        libro.SetActive(true);
                        break;

                    case "Esfera":
                        Debug.Log("Impacto con una Esfera: " + hit.collider.name);
                        // Acción específica para "Esfera"
                        break;

                    default:
                        break;
                }

                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }
        }

        
    }
}
