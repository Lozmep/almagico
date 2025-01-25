using DialogueSystem;
using Indicator;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Linq;

namespace EventManager
{
    public class EventManager : MonoBehaviour
    {
        [Header("Indicators")]
        private IndicatorManager indicatorManager;

        [Header("Events information")]
        public List<EventData> eventPool = new List<EventData>();
        public EventData currentEvent;
        public int currentNPC;
        public NPC currentNpcObject; 
        public bool isFarming;
        public List<EventData> currentEventPool = new List<EventData>();

        [Header("Event Management")]
        public AchievementSystem achievementSystem;

        [Header("Dialogue Management")]
        public DialogueManager dialogueManager;

        [Header("Initial Event Dialogue Management")]
        public EventDialogue eventDialogue;
        private List<int> completedEvents = new List<int>();

        [Header("Fade feature")]
        public FadeObject fade;
        public Image activatePanel;
        public TextMeshProUGUI activatePanelText;

        // Tiempo
        //private float timeSinceLastCheck = 0f;
        //private float eventCooldownTimer = 0f;
        //private float indicatorsAbove70Timer = 0f;
        private int lastEventId = 0;

        public bool eventInProgress = false;

        // Umbrales
        private const float globalThreshold = 200f;
        private const float cooldownDuration = 10f;
        public Dictionary<System.Func<bool>, System.Action> events;

        private void Awake()
        {
            indicatorManager = GetComponent<IndicatorManager>();
        }

        void Start()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "events.json");
            LoadEventsFromFile(path);
            //LoadEventsFromFile("Assets/Data/events.json");
            events = new Dictionary<System.Func<bool>, System.Action>
            {
                { () => ShouldTriggerEvent(indicatorManager.stressIndicator, true, IndicatorType.Stress), () => SelectEvent(IndicatorType.Stress) },
                { () => ShouldTriggerEvent(indicatorManager.selfCareIndicator, false, IndicatorType.SelfCare), () => SelectEvent(IndicatorType.SelfCare) },
                { () => ShouldTriggerEvent(indicatorManager.communicationIndicator, false, IndicatorType.Communication), () => SelectEvent(IndicatorType.Communication) }
            };
        }

        public IEnumerator CheckForEventActivationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);

                if (!eventInProgress)
                {
                    ValidateAndActivateEvent();
                }
            }
        }

        private void ValidateAndActivateEvent()
        {
            var shuffledKeys = new List<System.Func<bool>>(events.Keys);
            Shuffle(shuffledKeys);

            foreach (var condition in shuffledKeys)
            {
                if (condition.Invoke())
                {
                    events[condition].Invoke();
                    break;
                }
            }
        }

        private bool ShouldTriggerEvent(float indicatorValue, bool inverse, IndicatorType indicatorType)
        {
            var foundEvent = currentEventPool.FirstOrDefault(e => e.mainIndicator == indicatorType);

            if (foundEvent == null) {
                return false;
            }

            float prob = 0f;

            if ((inverse && indicatorValue < 10f) || (!inverse && indicatorValue > 90f))
            {
                prob = 10f;
            }
            else if ((inverse && indicatorValue >= 10f && indicatorValue < 30f) || (!inverse && indicatorValue > 70f && indicatorValue <= 90f))
            {
                prob = 15f;
            }
            else if ((inverse && indicatorValue >= 30f && indicatorValue < 50f) || (!inverse && indicatorValue > 50f && indicatorValue <= 70f))
            {
                prob = 30f;
            }
            else if ((inverse && indicatorValue >= 50f && indicatorValue < 70f) || (!inverse && indicatorValue > 30f && indicatorValue <= 50f))
            {
                prob = 50f;
            }
            else if ((inverse && indicatorValue >= 70f && indicatorValue < 90f) || (!inverse && indicatorValue > 10f && indicatorValue <= 30f))
            {
                prob = 70f;
            }
            else
            {
                prob = 100f;
            }

            float randomValue = Random.Range(0f, 100f);
            return randomValue <= prob;
        }

        private EventData SelectEvent(IndicatorType indicatorType)
        {
            List<EventData> shuffledEvents = new List<EventData>(currentEventPool);
            Shuffle(shuffledEvents);

            foreach (EventData e in shuffledEvents)
            {
                if (e.mainIndicator == indicatorType && lastEventId != e.id)
                {
                    currentEvent = e;
                    ActivateEvent();
                    return currentEvent;
                }
            }
            return null;
        }

        private void ActivateEvent()
        {
            StartCoroutine(fade.Fading(activatePanel, activatePanelText));
            currentNPC = Random.Range(1, 4);
            currentNpcObject = dialogueManager.npcList[currentNPC - 1];
            currentNpcObject.exclamation.SetActive(true);
            lastEventId = currentEvent.id;

            indicatorManager.IncreaseDecayRate = true;

            currentEvent.status = EventStatus.InProgress;
            eventInProgress = true;
            isFarming = false;
        }

        private void LoadEventsFromFile(string path)
        {
            if (File.Exists(path))
            {
                string jsonContent = File.ReadAllText(path);
                eventPool = JsonUtility.FromJson<EventDataArray>(jsonContent).eventList;
                string serializedEventPool = JsonUtility.ToJson(new EventDataArray { eventList = eventPool });
                currentEventPool = JsonUtility.FromJson<EventDataArray>(serializedEventPool).eventList;
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
                currentNpcObject.exclamation.SetActive(false);
                currentEvent.status = EventStatus.Completed;
                indicatorManager.IncreaseDecayRate = true;
                if (currentEvent.mainIndicator != IndicatorType.Communication || currentEvent.mainIndicator != IndicatorType.Farming) {
                    indicatorManager.modifyIndicators(currentEvent.stressImpact, currentEvent.selfCareImpact, currentEvent.communicationImpact, currentEvent.maintenanceImpact);
                } 
                eventInProgress = false;
                eventDialogue.isIndicated = false;
                currentEventPool.RemoveAll(e => e.id == currentEvent.id);
                if (currentEventPool.Count == 0)
                {
                    currentEventPool = eventPool;
                }
                checkEventAchievement();
                currentNPC = 0;
                currentEvent = null;
            }
        }

        private void checkEventAchievement() {
            if (!completedEvents.Contains(currentEvent.id)) { 
                completedEvents.Add(currentEvent.id);
            }
            if (completedEvents.Count == 6) {
                achievementSystem.CompareValuesInChildren(1);
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
        public DialogueScript dialogue;
        public IndicatorType mainIndicator;
        public EventStatus status;
    }
    
    [System.Serializable]
    public class EventDataArray
    {
        public List<EventData> eventList;
    }

}
