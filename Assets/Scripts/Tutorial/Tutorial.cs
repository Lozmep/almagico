using DialogueSystem;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public DialogueManager dialogueManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager.TutorialTxt();
    }

}
