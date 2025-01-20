using Indicator;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private float elapsedTime = 0f;
    public bool stopExecution = false;
    public IndicatorManager indicatorManager;
    public AchievementSystem achievementSystem;

    void Update()
    {
        if (stopExecution)
        {
            Debug.Log("Ejecución detenida.");
            return;
        }

        elapsedTime += Time.deltaTime;

        if (indicatorManager.globalIndicator < 200) {
            elapsedTime = 0f;            
        }

        if (Mathf.FloorToInt(elapsedTime) > 10)
        {
            Debug.Log("Han pasado 10 segundos.");
            achievementSystem.CompareValuesInChildren(0);
            achievementSystem.CompareValuesInChildren(1);
            achievementSystem.CompareValuesInChildren(2);
            stopExecution = true;
            gameObject.SetActive(false);
        }
    }
}
