using System.Collections;
using TMPro;
using UnityEngine;
using EventManager;

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
        
        [SerializeField] private float autoAdvanceTime = 3f;
        private bool isSelecting;
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

        void Start()
        {
            dialogueBox.SetActive(false);
        }

        public IEnumerator Speak(Lines[] dialogueLines)
        {
            dialogueBox.SetActive(true);
            isActive = true;

            foreach (Lines dialogue in dialogueLines)
            {
                txtName.text = dialogue.character;
                string[] textLines;

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
                    yield return StartCoroutine(TypeText(fullText));

                    float elapsedTime = 0f;
                    bool clicked = false;

                    while (elapsedTime < autoAdvanceTime && !clicked)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            clicked = true;
                        }
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
            }

            dialogueBox.SetActive(false);
            isSelecting = false;
            isActive = false;
            eventManager.CompleteEvent();
        }

        private IEnumerator TypeText(string text)
        {
            txtDialogue.text = "";

            for (int i = 0; i <= text.Length; i++)
            {
                txtDialogue.text = text.Substring(0, i);

                if (Input.GetKey(KeyCode.X))
                {
                    txtDialogue.text = text;
                    yield break;
                }

                yield return new WaitForSeconds(0.2f);
            }
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
}
