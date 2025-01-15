using System.Collections;
using UI;
using UnityEngine;

namespace Indicator {    
    public class IndicatorManager : MonoBehaviour
    {
        [Header("Indicators")]
        [Range(0f, 100f)] public float stressIndicator = 0f;
        [Range(0f, 100f)] public float selfCareIndicator = 100f;
        [Range(0f, 100f)] public float communicationIndicator = 100f;
        [Range(0f, 100f)] public float maintenanceIndicator = 100f;
        [Range(0f, 400f)] public float globalIndicator = 400f;

        [Header("Indicators UI")]
        public ProgressBar stressBar;
        public ProgressBar selfcareBar;
        public ProgressBar communicationBar;
        public ProgressBar maintenanceBar;
        public ProgressBar globalBar;

        [Range(0, 1)] private int threshold;
        private bool increaseDecayRate = false;
        private float decayRate = 2f;

        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        public bool IncreaseDecayRate
        {
            get { return increaseDecayRate; }
            set { increaseDecayRate = value; }
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

                stressBar.SetProgress(stressIndicator);
                selfcareBar.SetProgress(100f - selfCareIndicator);
                communicationBar.SetProgress(100f - communicationIndicator);
                maintenanceBar.SetProgress(100f - maintenanceIndicator);

                UpdateGlobalIndicator();
                Debug.Log($"[Indicadores] Estrés: {stressIndicator}, Autocuidado: {selfCareIndicator}, Comunicación: {communicationIndicator}, Mantenimiento: {maintenanceIndicator}, Global: {globalIndicator}");           
            }
        }

        private void UpdateGlobalIndicator()
        {
            globalIndicator = selfCareIndicator + communicationIndicator + maintenanceIndicator + (100 - stressIndicator);

            globalBar.SetProgress(400f - globalIndicator);

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

        public void modifyIndicators(float stressImpact, float selfCareImpact, float communicationImpact, float maintenanceImpact)
        {
            stressIndicator = Mathf.Clamp(stressIndicator - stressImpact, 0f, 100f);
            selfCareIndicator = Mathf.Clamp(selfCareIndicator + selfCareImpact, 0f, 100f);
            communicationIndicator = Mathf.Clamp(communicationIndicator + communicationImpact, 0f, 100f);
            maintenanceIndicator = Mathf.Clamp(maintenanceIndicator + maintenanceImpact, 0f, 100f);
        }

    }
}