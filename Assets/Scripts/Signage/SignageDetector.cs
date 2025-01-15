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
    public string type;
    private bool isSignaling;
    public bool playerInRange;

    private void Update()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            playerInRange = false;
            IsColliding(playerInRange);

            if (hitCollider.CompareTag(playerTag))
            {
                playerInRange = true;
                IsColliding(playerInRange);

                if (Input.GetKeyDown(KeyCode.X) && !dialogueManager.isActive) {
                    Debug.Log("XD");
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


    }

    private void IsColliding(bool playerInRange)
    {
        isSignaling = playerInRange;
        signObject.SetActive(isSignaling);
    }





}
