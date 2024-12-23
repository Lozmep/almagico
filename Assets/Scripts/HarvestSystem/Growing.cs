using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System.Collections;

public class Growing : MonoBehaviour
{
    public bool isGrowing;
    public bool isFree;

    public float timerDuration = 5f;

    void Start()
    {
        isGrowing = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrowing)
        {
            StartCoroutine(ActivateHarvest());
            isGrowing = false;
        }
        
    }

    IEnumerator ActivateHarvest()
    {
        // Espera durante el tiempo configurado
        yield return new WaitForSeconds(timerDuration);

        // Verifica si el objeto tiene al menos un hijo
        if (transform.childCount > 0)
        {
            
            Transform harvestTransform = transform.GetChild(1);
            harvestTransform.gameObject.SetActive(true);

            Transform plantTransform = transform.GetChild(0);
            plantTransform.gameObject.SetActive(false);


            Debug.Log("Hijo activado tras el temporizador.");
        }
        
    }
}
