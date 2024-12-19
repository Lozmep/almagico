using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EventManager
{
    public class EventManager : MonoBehaviour
    {
        [Header("Indicators")]
        [Range(0f, 100f)] public float stressIndicator = 0f;
        [Range(0f, 100f)] public float selfCareIndicator = 100f;
        [Range(0f, 100f)] public float communicationIndicator = 100f;
        [Range(0f, 100f)] public float maintenanceIndicator = 100f;
        [Range(0f, 300f)] public float globalIndicator;

        [Header("Events information")]
        public List<EventData> eventPool = new List<EventData>();
        public EventData currentEvent;
        public GameObject currentNPC;

        // Tiempo
        private float timeSinceLastCheck = 0f;
        private float eventCooldownTimer = 0f;
        private float indicatorsAbove70Timer = 0f;
        private int lastEventId = 0;

        public bool eventInProgress = false;

        // Umbrales
        private const float globalThreshold = 200f;
        private const float cooldownDuration = 10f;

        void Start()
        {
            LoadEventsFromFile("Assets/Data/events.json");
            StartCoroutine(DecreaseIndicatorsRoutine());
            StartCoroutine(CheckForEventActivationRoutine());
        }

        private IEnumerator DecreaseIndicatorsRoutine()
        {
            while (true)
            {
                if (eventInProgress)
                {
                    yield return new WaitForSeconds(10f); // Baja cada 15 segundos durante un evento activo
                }
                else
                {
                    yield return new WaitForSeconds(2f); // Baja cada 5 segundos cuando no hay eventos activos
                }

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
            globalIndicator = stressIndicator + selfCareIndicator + communicationIndicator + maintenanceIndicator;

            if (globalIndicator < globalThreshold)
            {
                Debug.Log("¡Umbral crítico alcanzado en el indicador global!");
            }
        }

        private IEnumerator CheckForEventActivationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f); // Chequeo cada 10 segundos
                ValidateAndActivateEvent();
            }
        }

        private void ValidateAndActivateEvent()
        {
            if (eventInProgress) return;

            // Reduce el cooldown si es necesario
            //if (eventCooldownTimer > 0)
            //{
            //    eventCooldownTimer -= Time.deltaTime;
            //    return;
            //}


            if (ShouldTriggerEvent(stressIndicator, true))
            {
                EventData eventSelected = SelectEvent(IndicatorType.Stress);
            } 
            else if (ShouldTriggerEvent(selfCareIndicator, false))
            {
                EventData eventSelected = SelectEvent(IndicatorType.SelfCare);
            }
            else if (ShouldTriggerEvent(communicationIndicator, false))
            {
                EventData eventSelected = SelectEvent(IndicatorType.Communication);
            }
            //else if (validateIndicatorProb(maintenanceIndicator, false))
            //{

            //}
            //// Busca un evento elegible
            //foreach (var e in eventPool)
            //{
            //    if (e.status == EventStatus.Pending && ShouldActivateEvent(e))
            //    {
            //        ActivateEvent(e);
            //        break;
            //    }
            //}
        }

        private bool ShouldTriggerEvent(float indicatorValue,  bool inverse) {
            float prob = 0f;

            if ((inverse && indicatorValue < 30f) || (!inverse && indicatorValue > 70f))
            {
                prob = 15f;
            }
            else if ((inverse && indicatorValue >= 30f && indicatorValue < 70f) || (!inverse && indicatorValue > 30f && indicatorValue <= 70f))
            {
                prob = 30f;
            }
            else
            {
                prob = 60f;
            }

            float randomValue = Random.Range(0f, 100f);
            return randomValue <= prob;
        }

        private EventData SelectEvent(IndicatorType indicatorType) {
            foreach (EventData e in eventPool)
            {
                if (e.mainIndicator == indicatorType) //&& lastEventId != e.id)
                {
                    currentEvent = e;
                    //currentNPC = ?
                    ActivateEvent();
                    return currentEvent;
                }
            }
            return null;        
        }

        private float GetEventActivationProbability(IndicatorType type)
        {
            float value = 0f;
            switch (type)
            {
                case IndicatorType.Stress:
                    value = stressIndicator;
                    break;
                case IndicatorType.SelfCare:
                    value = selfCareIndicator;
                    break;
                case IndicatorType.Communication:
                    value = communicationIndicator;
                    break;
                case IndicatorType.Farming:
                    value = maintenanceIndicator;
                    break;
            }

            if (value <= 30) return 70f; // Alta probabilidad
            if (value <= 70) return 40f; // Media probabilidad
            return 10f; // Baja probabilidad
        }

        private void ActivateEvent()
        {
            Debug.Log($"Evento activado: {currentEvent.name}");

            // Cambiar el estado del evento
            currentEvent.status = EventStatus.InProgress; 
            eventInProgress = true;

            // Inicia el proceso para finalizar el evento
            StartCoroutine(EndEventAfterDelay(5f)); // La duración del evento es de 5 segundos

            // Quizas debo validar en la corutina si el evento cambia a estado completado, ahi activo el end event
        }

        private IEnumerator EndEventAfterDelay(float duration)
        {
            yield return new WaitForSeconds(duration);

            // Aplicar impacto en los indicadores
            stressIndicator = Mathf.Clamp(stressIndicator + currentEvent.stressImpact, 0f, 100f);
            selfCareIndicator = Mathf.Clamp(selfCareIndicator + currentEvent.selfCareImpact, 0f, 100f);
            communicationIndicator = Mathf.Clamp(communicationIndicator + currentEvent.communicationImpact, 0f, 100f);
            maintenanceIndicator = Mathf.Clamp(maintenanceIndicator + currentEvent.maintenanceImpact, 0f, 100f);

            // Cambia el estado del evento a completado
            currentEvent.status = EventStatus.Completed; // Cuando valide la completitud esto ya no es necesario
            eventInProgress = false;

            // Reinicia el cooldown
            eventCooldownTimer = cooldownDuration;

            Debug.Log($"Evento finalizado: {currentEvent.name}");
            currentEvent = null;
        }

        private void LoadEventsFromFile(string path)
        {
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                eventPool = JsonUtility.FromJson<EventDataArray>(jsonContent).eventList;
            }
            else
            {
                Debug.LogError("No se encontró el archivo de eventos: " + path);
            }
        }
    }


    public enum IndicatorType
    {
        Stress,
        SelfCare,
        Communication,
        Farming
    }

    public enum EventStatus
    {
        Pending,
        InProgress,
        Completed
    }

    [System.Serializable]
    public class EventData
    {
        public int id;
        public string name;
        public float stressImpact;
        public float selfCareImpact;
        public float communicationImpact;
        public float maintenanceImpact;
        public IndicatorType mainIndicator;
        public EventStatus status;
    }

    [System.Serializable]
    public class EventDataArray
    {
        public List<EventData> eventList;
    }

}
