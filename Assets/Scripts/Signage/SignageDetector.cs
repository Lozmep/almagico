using DialogueSystem;
using SignalSystem;
using UnityEngine;

public class SignageDetector : MonoBehaviour
{
    public float radius; 
    public string playerTag = "Player"; 
    private bool isPlayerInRange = false;
    public GameObject signObject;
    public SignalDialogue signalDialogue;
    public DialogueManager dialogueManager;

    private void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        signObject.SetActive(false);
        


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(playerTag))
            {
                signObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.X) && !dialogueManager.isActive) {
                    Debug.Log("XD");
                    SignalObject signal = null;
                    foreach (var signalObject in signalDialogue.signalPool)
                    {
                        if (signalObject.type == "crop")
                        {
                            signal = signalObject;
                            break;
                        }
                    }
                    if (signal != null)
                    {
                        Debug.Log("Reconoce Signal");
                        StartCoroutine(dialogueManager.Speak(signal.dialogue.spanish));
                    }
                }

                


            }
        }

    }

   

}
