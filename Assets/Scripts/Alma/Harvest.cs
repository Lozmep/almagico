using UnityEngine;

public class Harvest : MonoBehaviour
{
    private TakeItemEvent takeItem;

    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;

    


    private void Awake()
    {
        takeItem = GetComponent<TakeItemEvent>();
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = transform.forward;
        Vector3 origin = transform.position;

        if (Input.GetKey(KeyCode.X) && takeItem.isCultivating)
        {

            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                Growing grow = hit.collider.GetComponent<Growing>();
                Crop crop = hit.collider.GetComponent<Crop>();

                if (crop.isCropFree)
                {
                    GameObject collidedObject = hit.collider.gameObject;
                    Transform childTransform = collidedObject.transform.GetChild(0);
                    childTransform.gameObject.SetActive(true);

                    takeItem.semillas.SetActive(false);

                    takeItem.isFree = true;
                    crop.isCropFree = false;
                    takeItem.isCultivating = false;

                    if (grow != null)
                    {
                        grow.isGrowing = true;
                        grow = null;
                    }
                }

              

                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }
        }


    }
}
