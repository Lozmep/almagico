using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private TakeItemEvent takeItem;

    public GameObject[] childrenArray;

    public float maxDistance = 1f;
    public LayerMask layerMask;
    public float verticalDistance = 0.8f;

    private void Awake()
    {
        takeItem = GetComponentInParent<TakeItemEvent>();
    }
    void Start()
    {
        

        int childCount = transform.childCount; 
        childrenArray = new GameObject[childCount]; 

        // Llena el arreglo con los hijos
        for (int i = 0; i < childCount; i++)
        {
            childrenArray[i] = transform.GetChild(i).gameObject;
        }

    }

    void Update()
    {

        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0); ;

        if (Input.GetKey(KeyCode.X))
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                DeactivateChildren();
                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.gray);
            }
        }


    }

    public void DeactivateChildren()
    {
        foreach (GameObject child in childrenArray)
        {
            if (child != null && child.CompareTag("CurrentItem"))
            {
                takeItem.isFree = true;
                takeItem.isCultivating = false;
                takeItem.currentItemID = 0;
                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.gameObject.activeSelf)
                    {
                        grandchild.gameObject.SetActive(false);
                    }
                }
            }
        }

    }
}
