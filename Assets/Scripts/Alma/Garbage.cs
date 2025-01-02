using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private TakeItemEvent takeItem;

    public GameObject[] childrenArray;

    public float maxDistance = 1f;
    public LayerMask layerMask;

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
        Vector3 origin = transform.position;

        if (Input.GetKey(KeyCode.X))
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                DeactivateChildren();
                takeItem.isFree = true;
                takeItem.isCultivating = false;
                takeItem.currentItemID = 0;
                

                Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
            }
        }


    }

    public void DeactivateChildren()
    {
        foreach (GameObject child in childrenArray)
        {
            if (child != null) 
            {
                child.SetActive(false); 
            }
        }

    }
}
