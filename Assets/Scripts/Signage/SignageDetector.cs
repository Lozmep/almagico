using DialogueSystem;
using SignalSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SignageDetector : MonoBehaviour
{
    public float radius; 
    public string playerTag = "Player"; 
    public GameObject signObject;
    public SignalDialogue signalDialogue;
    public DialogueManager dialogueManager;
    public TakeItemEvent takeItem;
    public string type;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            signObject.SetActive(true);

            if (Input.GetKey(KeyCode.X) && !dialogueManager.isActive && takeItem.isFree)
            {
                SignalObject signal = null;
                foreach (var signalObject in signalDialogue.signalPool)
                {
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
