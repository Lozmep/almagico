using DialogueSystem;
using SignalSystem;
using UnityEngine;
using TMPro;
using System;

public class SignageDetector : MonoBehaviour
{
   
    public string playerTag = "Player"; 
    public GameObject signObject;
    public SignalDialogue signalDialogue;
    public DialogueManager dialogueManager;
    public TakeItemEvent takeItem;
    public string type;

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProUGUI textType;
    [SerializeField] private TextMeshProUGUI textSignal;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            signObject.SetActive(true);
            string dialogueManagerBool = dialogueManager.isActive.ToString();
            string takeItemBoolean = takeItem.isFree.ToString();
            string getKeyBoolean = Input.GetKey(KeyCode.X).ToString();

            textMeshPro.text = "El dialogo esta " + dialogueManagerBool + ", las manos estan " + takeItemBoolean + ", la tecla esta " + getKeyBoolean;

            if (Input.GetKey(KeyCode.X) && !dialogueManager.isActive && takeItem.isFree)
            {
                textType.text = type;
                SignalObject signal = null;
                foreach (var signalObject in signalDialogue.signalPool)
                {
                    textSignal.text = signalObject.type.ToString();
                    if (signalObject.type == type)
                    {
                        signal = signalObject;
                        break;
                    }
                }
                
                if (signal != null)
                {
                    StartCoroutine(dialogueManager.Speak(signal.dialogue.spanish));
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        signObject.SetActive(false);
    }

}
