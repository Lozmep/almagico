using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class ChoiceManager : MonoBehaviour
    {

        public Button[] choiceButtons;
        public TextMeshProUGUI[] choiceTexts;
        public DialogueManager dialogueManager;
        public int selectedButtonIndex = 0;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetChoiceTexts(DialogueChoice[] choices) {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < choices.Length)
                {
                    choiceTexts[i].text = choices[i].text;
                    choiceButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }
        }

        public void ButtonSelected(int index)
        {
            Debug.Log($"Se selecciono el boton: {index} con texto: {choiceTexts[index].text}");
            selectedButtonIndex = index;
            dialogueManager.IsSelecting = false;
        }
    }
}

