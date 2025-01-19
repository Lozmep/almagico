using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.LightTransport;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;
using Indicator;

public class AchievementSystem : MonoBehaviour
{

    [SerializeField] private GameObject parentObject; // Objeto padre que contiene los hijos
    public bool newState;
    public IndicatorManager indicatorManager;
    public TextController textController;

    private void Awake()
    {
        newState = false;
        parentObject.SetActive(newState);
    }
    public void ToggleVisibility()
    {
        if (parentObject != null)
        {
            newState = !newState;
            parentObject.SetActive(newState);

        }
    }

    public void SetAlphaTo100(GameObject panel)
    {
        Image panelImage = panel.GetComponent<Image>();
        Color panelColor = panelImage.color;
        panelColor.a = 1f; 
        panelImage.color = panelColor;

        int childCount = panel.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {

            Transform child = panel.transform.GetChild(i);
            RawImage rawImage = child.GetComponent<RawImage>();

            if (rawImage != null)
            {
                Color color = rawImage.color;
                color.a = 1f; 
                rawImage.color = color;
            }

        }

    }

    public void CompareValuesInChildren(int targetValue)
    {
        if (parentObject != null)
        {
            int childCount = parentObject.transform.childCount; 

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parentObject.transform.GetChild(i); 

                
                ChildData childData = child.GetComponent<ChildData>();

                if (childData != null)
                {
                    if (childData.value == targetValue)
                    {
                        Debug.Log("Entró");
                        Debug.Log(child.gameObject.name);   
                        SetAlphaTo100(child.gameObject);
                        Debug.Log("Se logró gente");
                        indicatorManager.modifyIndicators(10f, 10f, 10f, 10f);
                        textController.ShowTextWithFade("Has desbloqueado un logro!!!");
                    }

                } else
                {
                    Debug.Log("No se ha encontra un panel con el mimso ID");
                }

            }
        } else
        {
            Debug.Log("No se ha encontrado al padre");
        }

    }

}
