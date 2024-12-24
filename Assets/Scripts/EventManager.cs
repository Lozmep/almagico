using Indicator;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EventManager
{
    public class EventManager : MonoBehaviour
    {
        [Header("Indicators")]
        public IndicatorManager indicatorManager;

        [Header("Events information")]
        public List<EventData> eventPool = new List<EventData>();
        public EventData currentEvent;
        public int currentNPC;

        // Tiempo
        //private float timeSinceLastCheck = 0f;
        //private float eventCooldownTimer = 0f;
        //private float indicatorsAbove70Timer = 0f;
        private int lastEventId = 0;

        public bool eventInProgress = false;

        // Umbrales
        private const float globalThreshold = 200f;
        private const float cooldownDuration = 10f;
        private Dictionary<System.Func<bool>, System.Action> events;

        private void Awake()
        {
            indicatorManager = GetComponent<IndicatorManager>();
        }

        void Start()
        {
            LoadEventsFromFile("Assets/Data/events.json");
            events = new Dictionary<System.Func<bool>, System.Action>
            {
                { () => ShouldTriggerEvent(indicatorManager.stressIndicator, true), () => SelectEvent(IndicatorType.Stress) },
                { () => ShouldTriggerEvent(indicatorManager.selfCareIndicator, false), () => SelectEvent(IndicatorType.SelfCare) },
                { () => ShouldTriggerEvent(indicatorManager.communicationIndicator, false), () => SelectEvent(IndicatorType.Communication) },
                { () => ShouldTriggerEvent(indicatorManager.maintenanceIndicator, false), () => SelectEvent(IndicatorType.Farming) }
            };
            StartCoroutine(CheckForEventActivationRoutine());
        }

        private IEnumerator CheckForEventActivationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f); // Chequeo cada 10 segundos

                if (!eventInProgress)
                {
                    ValidateAndActivateEvent();
                }
            }
        }

        private void ValidateAndActivateEvent()
        {
            // Reduce el cooldown si es necesario
            //if (eventCooldownTimer > 0)
            //{
            //    eventCooldownTimer -= Time.deltaTime;
            //    return;
            //}
            // Mezclar las claves (condiciones)

            // Evaluar condiciones en orden aleatorio
            var shuffledKeys = new List<System.Func<bool>>(events.Keys);
            foreach (var condition in shuffledKeys)
            {
                if (condition.Invoke())
                {
                    events[condition].Invoke();
                    Shuffle(shuffledKeys);
                    break; // Salir al cumplirse la primera condición
                }
            }

            // SE CAMBIA PARA QUE LA PRIORIDAD NO SIEMPRE SEA LA MISMA
            //if (ShouldTriggerEvent(stressIndicator, true))
            //{
            //    EventData eventSelected = SelectEvent(IndicatorType.Stress);
            //}
            //else if (ShouldTriggerEvent(selfCareIndicator, false))
            //{
            //    EventData eventSelected = SelectEvent(IndicatorType.SelfCare);
            //}
            //else if (ShouldTriggerEvent(communicationIndicator, false))
            //{
            //    EventData eventSelected = SelectEvent(IndicatorType.Communication);
            //}
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

        private bool ShouldTriggerEvent(float indicatorValue, bool inverse)
        {
            float prob = 0f;

            if ((inverse && indicatorValue < 30f) || (!inverse && indicatorValue > 70f))
            {
                Debug.Log("PROP 15");
                prob = 15f;
            }
            else if ((inverse && indicatorValue >= 30f && indicatorValue < 70f) || (!inverse && indicatorValue > 30f && indicatorValue <= 70f))
            {
                Debug.Log("PROP 30");
                prob = 30f;
            }
            else
            {
                Debug.Log("PROP 65");
                prob = 60f;
            }

            float randomValue = Random.Range(0f, 100f);
            return randomValue <= prob;
        }

        private EventData SelectEvent(IndicatorType indicatorType)
        {
            foreach (EventData e in eventPool)
            {
                if (e.mainIndicator == indicatorType && lastEventId != e.id)
                {
                    currentEvent = e;
                    //currentNPC = ?
                    ActivateEvent();
                    return currentEvent;
                }
            }
            return null;
        }

        private void ActivateEvent()
        {
            Debug.Log($"Evento activado: {currentEvent.name}");

            // Cambiar el estado del evento
            currentEvent.status = EventStatus.InProgress;
            eventInProgress = true;

            // Inicia el proceso para finalizar el evento
            //StartCoroutine(EndEventAfterDelay(5f)); // La duración del evento es de 5 segundos
            StartCoroutine(EndEventWhenStatusChanges()); // La duración del evento es de 5 segundos

            // Quizas debo validar en la corutina si el evento cambia a estado completado, ahi activo el end event
        }

        private IEnumerator EndEventWhenStatusChanges()
        {
            while (currentEvent != null && currentEvent.status != EventStatus.Completed)
            {
                yield return null; // Espera hasta el siguiente frame.
            }

            // Aplicar impacto en los indicadores
            indicatorManager.stressIndicator = Mathf.Clamp(indicatorManager.stressIndicator + currentEvent.stressImpact, 0f, 100f);
            indicatorManager.selfCareIndicator = Mathf.Clamp(indicatorManager.selfCareIndicator + currentEvent.selfCareImpact, 0f, 100f);
            indicatorManager.communicationIndicator = Mathf.Clamp(indicatorManager.communicationIndicator + currentEvent.communicationImpact, 0f, 100f);
            indicatorManager.maintenanceIndicator = Mathf.Clamp(indicatorManager.maintenanceIndicator + currentEvent.maintenanceImpact, 0f, 100f);

            // Cambiar el estado del evento
            eventInProgress = false;
            //eventCooldownTimer = cooldownDuration;

            Debug.Log($"Evento finalizado: {currentEvent.name}");
            currentEvent = null;
        }


        // SE MODIFICA PARA QUE NO SEA CADA CIERTO TIEMPO, SINO CUANDO TERMINA EL EVENTO
        //private IEnumerator EndEventAfterDelay(float duration)
        //{
        //    yield return new WaitForSeconds(duration);

        //    // Aplicar impacto en los indicadores
        //    stressIndicator = Mathf.Clamp(stressIndicator + currentEvent.stressImpact, 0f, 100f);
        //    selfCareIndicator = Mathf.Clamp(selfCareIndicator + currentEvent.selfCareImpact, 0f, 100f);
        //    communicationIndicator = Mathf.Clamp(communicationIndicator + currentEvent.communicationImpact, 0f, 100f);
        //    maintenanceIndicator = Mathf.Clamp(maintenanceIndicator + currentEvent.maintenanceImpact, 0f, 100f);

        //    // Cambia el estado del evento a completado
        //    currentEvent.status = EventStatus.Completed; // Cuando valide la completitud esto ya no es necesario
        //    eventInProgress = false;

        //    // Reinicia el cooldown
        //    eventCooldownTimer = cooldownDuration;

        //    Debug.Log($"Evento finalizado: {currentEvent.name}");
        //    currentEvent = null;
        //}



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

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        public void CompleteEvent()
        {
            if (currentEvent != null)
            {
                currentEvent.status = EventStatus.Completed;
                Debug.Log($"Evento {currentEvent.name} completado.");
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
