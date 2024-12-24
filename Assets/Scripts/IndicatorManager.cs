using System.Collections;
using UnityEngine;

namespace Indicator {    
    public class IndicatorManager : MonoBehaviour
    {
        [Header("Indicators")]
        [Range(0f, 100f)] public float stressIndicator = 0f;
        [Range(0f, 100f)] public float selfCareIndicator = 100f;
        [Range(0f, 100f)] public float communicationIndicator = 100f;
        [Range(0f, 100f)] public float maintenanceIndicator = 100f;
        [Range(0f, 300f)] public float globalIndicator;

        [Range(0, 1)] private int threshold;
        private bool increaseDecayRate = true;
        private float decayRate = 2f;

        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        public bool IncreaseDecayRate
        {
            get { return IncreaseDecayRate; }
            set { IncreaseDecayRate = value; }
        }

        void Start()
        {
            StartCoroutine(DecreaseIndicatorsRoutine());
        }

        private IEnumerator DecreaseIndicatorsRoutine()
        {
            while (true)
            {
                decayRate = increaseDecayRate ? 10f : 2f;
                yield return new WaitForSeconds(decayRate);

                stressIndicator = Mathf.Clamp(stressIndicator + 2f, 0f, 100f);
                selfCareIndicator = Mathf.Clamp(selfCareIndicator - 1f, 0f, 100f);
                communicationIndicator = Mathf.Clamp(communicationIndicator - 1f, 0f, 100f);
                maintenanceIndicator = Mathf.Clamp(maintenanceIndicator - 1f, 0f, 100f);

                UpdateGlobalIndicator();
                Debug.Log($"[Indicadores] Estrés: {stressIndicator}, Autocuidado: {selfCareIndicator}, Comunicación: {communicationIndicator}, Mantenimiento: {maintenanceIndicator}, Global: {globalIndicator}");
            }
        }

        private void UpdateGlobalIndicator()
        {
            globalIndicator = selfCareIndicator + communicationIndicator + maintenanceIndicator + (100 - stressIndicator);

            if (globalIndicator < 200)
            {
                Debug.Log("¡Umbral crítico alcanzado en el indicador global!");
                threshold = 1;
            } 
            else
            {
                threshold = 0;
            }
        }

        public void modifyIndicators(float stressValue, float selfCareValue, float communicationValue, float maintenanceValue)
        {
            stressIndicator -= stressValue;
            selfCareIndicator += selfCareValue;
            communicationIndicator += communicationValue;
            maintenanceIndicator += maintenanceValue;            
        }

    }
}