using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GiveItemEvent : MonoBehaviour
{

    private TakeItemEvent takeItem;


    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public LayerMask layerDeliveryMask;

    [Header("Event Management")]
    public EventManager.EventManager eventManager;


    private void Awake()
    {
        takeItem = GetComponent<TakeItemEvent>();
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

                if (npc.ID != eventManager.currentNPC) return;

                switch (takeItem.currentItemID)
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
                        Debug.Log("Gracias por la venta! ");
                        break;

                    default:
                        break;
                }



                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }

            if (Physics.Raycast(origin, direction, out RaycastHit shoot, maxDistance, layerDeliveryMask))
            {
                Debug.Log("Entra");
                Delivery delivery = shoot.collider.GetComponent<Delivery>();


                if (delivery.deliveryID == takeItem.currentItemID)
                {
                    Debug.Log("Gracias por la venta!");
                    takeItem.cultivo.SetActive(false);
                    takeItem.currentItemID = 0;
                    takeItem.isFree = true;
                    delivery.deliverySum++;
                    Debug.Log(delivery.deliverySum);
                }

            }
        }

    }
}
