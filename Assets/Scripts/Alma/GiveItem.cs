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
    public LayerMask layerDeliveryMask;




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

                if(takeItem.currentItemID != value)
                {
                    return;
                }



                switch (value)
                {
                    case 1:
                        takeItem.tinto.SetActive(false);
                        takeItem.isFree = true;
                        break;

                    case 2:
                        takeItem.libro.SetActive(false);
                        takeItem.isFree = true;
                        break;

                    case 3:
                        break;

                    default:
                        break;
                }

 

                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }

            if (Physics.Raycast(origin, direction, out RaycastHit shoot, maxDistance, layerDeliveryMask))
            {
                Delivery delivery = shoot.collider.GetComponent<Delivery>();
                

                if (delivery.deliveryID == takeItem.currentItemID)
                {
                    takeItem.cultivo.SetActive(false);
                    takeItem.currentItemID = 0;
                    takeItem.isFree = true;
                    delivery.deliverySum++;
                }

            }
        }

    }
}
