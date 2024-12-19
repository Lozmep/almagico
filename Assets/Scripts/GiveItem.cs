using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GiveItem : MonoBehaviour
{

    private TakeItem takeItem;

    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;

    


    private void Awake()
    {
        takeItem = GetComponent<TakeItem>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position;

        if (Input.GetKey(KeyCode.X))
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {

                NPC npc = hit.collider.GetComponent<NPC>();
                int value = npc.ID;


                switch (value)
                {
                    case 1:
                        Debug.Log("Gracias por el tinto! Seamos amigos");
                        takeItem.tinto.SetActive(false);
                        takeItem.isFree = true;
                        break;

                    case 2:
                        Debug.Log("Leer es saber");
                        takeItem.libro.SetActive(false);
                        takeItem.isFree = true;
                        break;

                    case 3:
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
