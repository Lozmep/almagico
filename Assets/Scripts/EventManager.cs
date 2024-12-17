using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EventManager
{
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

            // Pool de eventos
            private List<EventData> eventPool = new List<EventData>();

            // Tiempo
            private float timeSinceLastCheck = 0f;
            private float eventCooldownTimer = 0f;
            private float indicatorsAbove70Timer = 0f;

            private bool eventInProgress = false;

            // Umbrales
            private const float globalThreshold = 50f;
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
                // No activar eventos si uno ya está en progreso
                if (eventInProgress) return;

                // Reduce el cooldown si es necesario
                if (eventCooldownTimer > 0)
                {
                    eventCooldownTimer -= Time.deltaTime;
                    return;
                }

                // Busca un evento elegible
                foreach (var e in eventPool)
                {
                    if (e.status == EventStatus.Pending && ShouldActivateEvent(e))
                    {
                        ActivateEvent(e);
                        break; // Solo activa un evento a la vez
                    }
                }
            }

            private bool ShouldActivateEvent(EventData e)
            {
                float probability = GetEventActivationProbability(e.mainIndicator);
                float randomValue = Random.Range(0f, 100f);
                return randomValue <= probability;
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

            private void ActivateEvent(EventData e)
            {
                Debug.Log($"Evento activado: {e.name}");

                // Aplicar impacto en los indicadores
                stressIndicator = Mathf.Clamp(stressIndicator + e.stressImpact, 0f, 100f);
                selfCareIndicator = Mathf.Clamp(selfCareIndicator + e.selfCareImpact, 0f, 100f);
                communicationIndicator = Mathf.Clamp(communicationIndicator + e.communicationImpact, 0f, 100f);
                maintenanceIndicator = Mathf.Clamp(maintenanceIndicator + e.maintenanceImpact, 0f, 100f);

                // Cambiar el estado del evento
                e.status = EventStatus.InProgress;
                eventInProgress = true;

                // Inicia el proceso para finalizar el evento
                StartCoroutine(EndEventAfterDelay(e, 3f)); // La duración del evento es de 15 segundos
            }

            private IEnumerator EndEventAfterDelay(EventData e, float duration)
            {
                yield return new WaitForSeconds(duration);

                // Cambia el estado del evento a completado
                e.status = EventStatus.Completed;
                eventInProgress = false;

                // Reinicia el cooldown
                eventCooldownTimer = cooldownDuration;

                Debug.Log($"Evento finalizado: {e.name}");
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