using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Indicator;
using GameSystem;

public class AchievementSystem : MonoBehaviour
{

    [SerializeField] private GameObject parentObject; // Objeto padre que contiene los hijos
    public bool newState;
    public bool isGameFinished;
    public IndicatorManager indicatorManager;
    public TextController textController;
    public GameStatusManager gameStatusManager;
    public ChildData childData;
    private List<int> completedAchievements = new List<int>();

    private void Awake()
    {
        newState = false;
        parentObject.SetActive(newState);
    }
    public void ToggleVisibility()
    {
        if (parentObject != null && !gameStatusManager.isOver)
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
                        SetAlphaTo100(child.gameObject);
                        indicatorManager.modifyIndicators(7f, 6f, 6f, 6f);
                        textController.ShowTextWithFade("Has desbloqueado un logro!!!");
                        CheckAchievements(targetValue);
                        childData.emptyStar.gameObject.SetActive(false);
                        childData.star.gameObject.SetActive(true);
                    }

                }

            }
        }

    }

    private void CheckAchievements(int value)
    {
        if (!completedAchievements.Contains(value))
        {
            completedAchievements.Add(value);
        }
        if (completedAchievements.Count == 3)
        {
            isGameFinished = true;
        }
    }

}
