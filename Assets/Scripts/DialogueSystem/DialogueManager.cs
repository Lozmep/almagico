using System.Collections;
using TMPro;
using UnityEngine;
using SignalSystem;
using Indicator;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Dialogue Settings")]
        public GameObject dialogueBox;
        public TextMeshProUGUI txtDialogue;
        public TextMeshProUGUI txtName;
        public bool isActive;
        public EventManager.EventManager eventManager;
        public TxtLines[] NotMineDialogue;
        public ChoiceManager choiceManager;
        public IndicatorManager indicatorManager;
        public NPC[] npcList;
        public TxtLines[] tutorialDialogue;

        [SerializeField] private float autoAdvanceTime = 3f;
        private bool isSelecting;
        private bool skipTyping;
        private int selectedOption = 1;
        // Componente para opciones -- este es el que cambia is selection
        private DialogueScript _dialogue;
        public DialogueScript Dialogue
        {
            get { return _dialogue; }
            set { _dialogue = value; }
        }
        public bool IsSelecting
        {
            get { return isSelecting; }
            set { isSelecting = value; }
        }
        public int SelectedOption
        {
            get { return selectedOption; }
            set { selectedOption = value; }
        }

        public IEnumerator Speak(Lines[] dialogueLines)
        {
            dialogueBox.SetActive(true);
            isActive = true;

            StartCoroutine(DetectKeyPress());

            foreach (Lines dialogue in dialogueLines)
            {
                txtName.text = dialogue.character;
                if (eventManager!= null && eventManager.currentNpcObject != null)
                {
                    txtName.text = dialogue.character.Replace("NPC", eventManager.currentNpcObject.npcName);
                }
                string[] textLines;
                txtDialogue.text = "";

                if (!dialogue.triggerChoices)
                {
                    if (dialogue.text.Length > 0)
                    {
                        textLines = dialogue.text;
                    }
                    else
                    {
                        switch (selectedOption)
                        {
                            case 1:
                                textLines = dialogue.conditionalText1;
                                break;
                            case 2:
                                textLines = dialogue.conditionalText2;
                                break;
                            case 3:
                                textLines = dialogue.conditionalText3;
                                break;
                            case 4:
                                textLines = dialogue.conditionalText4;
                                break;
                            default:
                                textLines = new string[0];
                                break;
                        }
                    }

                    foreach (string fullText in textLines)
                    {
                        txtDialogue.text = "";

                        for (int i = 0; i <= fullText.Length; i++)
                        {
                            txtDialogue.text = fullText.Substring(0, i);
                            if (skipTyping)
                            {
                                txtDialogue.text = fullText;
                                yield return new WaitForSeconds(0.1f);
                                break;
                            }
                            yield return new WaitForSeconds(0.1f);
                        }

                        float elapsedTime = 0f;

                        while (elapsedTime < autoAdvanceTime)
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                elapsedTime = autoAdvanceTime;
                            }
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        skipTyping = false;
                    }
                } else 
                {
                    isSelecting = true;
                    txtDialogue.text = dialogue.text[0];
                    choiceManager.SetChoiceTexts(dialogue.choices);
                    while (isSelecting) {
                        yield return new WaitForSeconds(0.1f);
                    }
                    selectedOption = choiceManager.selectedButtonIndex + 1;
                    DialogueChoice choice = dialogue.choices[choiceManager.selectedButtonIndex];
                    indicatorManager.modifyIndicators(choice.stressImpact, choice.selfCareImpact, choice.communicationImpact, choice.maintenanceImpact);
                    choiceManager.gameObject.SetActive(false);

                }    
            }

            dialogueBox.SetActive(false);
            isSelecting = false;
            isActive = false;
            eventManager.CompleteEvent();
        }

        public IEnumerator Speak(SignalLines[] dialogueLines)
        {
            dialogueBox.SetActive(true);
            isActive = true;

            StartCoroutine(DetectKeyPress());

            print("Dialog1");
            print(dialogueLines);

            foreach (SignalLines dialogue in dialogueLines)
            {
                txtName.text = dialogue.character;
                if (eventManager != null && eventManager.currentNpcObject != null)
                {
                    txtName.text = dialogue.character.Replace("NPC", eventManager.currentNpcObject.npcName);
                }
                string[] textLines;

                textLines = dialogue.text;

                print("Dialog");
                print(textLines);

                foreach (string fullText in textLines)
                {
                    txtDialogue.text = "";
                    skipTyping = false;

                    for (int i = 0; i <= fullText.Length; i++)
                    {
                        txtDialogue.text = fullText.Substring(0, i);
                        if (skipTyping)
                        {
                            txtDialogue.text = fullText;
                            yield return new WaitForSeconds(0.1f);
                            break;
                        }
                        yield return new WaitForSeconds(0.1f);
                    }

                    float elapsedTime = 0f;

                    while (elapsedTime < autoAdvanceTime)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            elapsedTime = autoAdvanceTime;
                        }
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                }
            }

            dialogueBox.SetActive(false);
            isActive = false;
        }

        private IEnumerator DetectKeyPress()
        {
            while (isActive)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    skipTyping = true;
                }
                yield return null;
            }
        }

        public IEnumerator Speak(InitEventLines[] dialogueLines)
        {
            dialogueBox.SetActive(true);
            isActive = true;

            StartCoroutine(DetectKeyPress());

            print("Dialog1");
            print(dialogueLines);

            foreach (InitEventLines dialogue in dialogueLines)
            {
                txtName.text = dialogue.character;
                if (eventManager!= null && eventManager.currentNpcObject != null)
                {
                    txtName.text = dialogue.character.Replace("NPC", eventManager.currentNpcObject.npcName);
                }
                string[] textLines;

                textLines = dialogue.text;

                print("Dialog");
                print(textLines);

                foreach (string fullText in textLines)
                {
                    txtDialogue.text = "";
                    skipTyping = false;

                    for (int i = 0; i <= fullText.Length; i++)
                    {
                        txtDialogue.text = fullText.Substring(0, i);
                        if (skipTyping)
                        {
                            txtDialogue.text = fullText;
                            yield return new WaitForSeconds(0.1f);
                            break;
                        }
                        yield return new WaitForSeconds(0.1f);
                    }

                    float elapsedTime = 0f;

                    while (elapsedTime < autoAdvanceTime)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            elapsedTime = autoAdvanceTime;
                        }
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                }
            }

            dialogueBox.SetActive(false);
            isActive = false;

            if (eventManager.currentEvent.id == 3 || eventManager.currentEvent.id == 4)
            {
                if (eventManager.eventDialogue.isIndicated)
                {
                    eventManager.isFarming = true;
                    StartCoroutine(FarmingDuration());
                }
            }
            else
            {
                eventManager.isFarming = false;
            }
        }

        public IEnumerator Speak(TxtLines[] dialogueLines, bool isTutorial)
        {
            dialogueBox.SetActive(true);
            isActive = true;

            StartCoroutine(DetectKeyPress());

            print("Dialog1");
            print(dialogueLines);

            foreach (TxtLines dialogue in dialogueLines)
            {
                txtName.text = dialogue.character;
                if (eventManager != null && eventManager.currentNpcObject != null)
                {
                    txtName.text = dialogue.character.Replace("NPC", eventManager.currentNpcObject.npcName);
                }
                string[] textLines;

                textLines = dialogue.texts;

                print("Dialog");
                print(textLines);

                foreach (string fullText in textLines)
                {
                    txtDialogue.text = "";
                    skipTyping = false;

                    for (int i = 0; i <= fullText.Length; i++)
                    {
                        txtDialogue.text = fullText.Substring(0, i);
                        if (skipTyping)
                        {
                            txtDialogue.text = fullText;
                            yield return new WaitForSeconds(0.1f);
                            break;
                        }
                        yield return new WaitForSeconds(0.1f);
                    }

                    float elapsedTime = 0f;

                    while (elapsedTime < autoAdvanceTime)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            elapsedTime = autoAdvanceTime;
                        }
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                }
            }

            dialogueBox.SetActive(false);
            isActive = false;

            if (isTutorial)
            {
                StartCoroutine(indicatorManager.DecreaseIndicatorsRoutine());
                StartCoroutine(eventManager.CheckForEventActivationRoutine());
                isTutorial = false;
            }
        }

        public void IntTxt()
        {
            StartCoroutine(Speak(NotMineDialogue, false));
        }

        private IEnumerator FarmingDuration()
        {
            npcList[eventManager.currentNPC - 1].gameObject.SetActive(false);
            yield return new WaitForSeconds(30f);
            eventManager.isFarming = false;
            npcList[eventManager.currentNPC - 1].gameObject.SetActive(true);
            eventManager.CompleteEvent();
        }

        public void TutorialTxt()
        {
            StartCoroutine(Speak(tutorialDialogue, true));
        }
    }


    [System.Serializable]

    public class DialogueScript
    {
        public Lines[] spanish;
        public Lines[] english;
    }

    [System.Serializable]
    public class Lines
    {
        public string character;
        public string[] text;
        public string[] conditionalText1;
        public string[] conditionalText2;
        public string[] conditionalText3;
        public string[] conditionalText4;
        public bool triggerChoices;
        public DialogueChoice[] choices;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string text;
        public float stressImpact;
        public float selfCareImpact;
        public float communicationImpact;
        public float maintenanceImpact;
    }

    [System.Serializable]
    public class TxtLines
    {
        public string[] texts;
        public string character;
    }
}
