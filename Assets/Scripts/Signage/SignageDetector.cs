using UnityEngine;

public class SignageDetector : MonoBehaviour
{
    public float radius; 
    public string playerTag = "Player"; 
    private bool isPlayerInRange = false;
    public GameObject signObject;

    private void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        signObject.SetActive(false);
        


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(playerTag))
            {
                signObject.SetActive(true);
            
            }
        }

    }

   

}
