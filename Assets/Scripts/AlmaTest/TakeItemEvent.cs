using DialogueSystem;
using EventManager;
using System.Collections.Generic;
using UnityEngine;

public class TakeItemEvent : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 1f;
    public float verticalDistance = 0.8f;
    public LayerMask layerMask;
    public LayerMask layerCropMask;
    public LayerMask layerNPCMask;

    [Header("GameObjects")]
    public GameObject tinto;
    public GameObject libro;
    public GameObject semillas;
    public GameObject cultivo;

    [Header("States")]
    public bool isFree = true;
    public bool isCultivating = false;

    [Header("Event Management")]
    public int currentItemID;
    public EventManager.EventManager eventManager;

    [Header("Dialogue Management")]
    public DialogueManager dialogueManager;

    [Header("Initial Event Dialogue Management")]
    public EventDialogue eventDialogue;

    private void Awake()
    {
        eventDialogue = GetComponent<EventDialogue>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.X) && isFree)
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hitDialog, maxDistance, layerNPCMask))
        {
            HandleDialog(hitDialog);
        }

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
        {
            HandleHit(hit);
        }

        if (Physics.Raycast(origin, direction, out RaycastHit cropHit, maxDistance, layerCropMask))
        {
            HandleCropHit(cropHit);
        }

        Debug.DrawRay(origin, direction * maxDistance, Color.red);
    }

    private void HandleHit(RaycastHit hit)
    {
        Item item = hit.collider.GetComponent<Item>();
        Debug.Log("Item" + item.name);
        if (item == null) return;

        currentItemID = item.itemID;
        string objectTag = hit.collider.tag;

        if (eventManager && eventManager.currentEvent != null && eventDialogue.isIndicated)
        {
            ValidateInteraction(eventManager.currentEvent.id, objectTag, hit);
        }

        if (isFree)
        {
            HandleTagAction(objectTag, hit);
        }
    }

    private void HandleDialog(RaycastHit hit)
    {
        if (dialogueManager.isActive)
        {
            Debug.LogWarning("Dialogue already active, ignoring new interaction.");
            return;
        }
        NPC npc = hit.collider.GetComponent<NPC>();

        if (eventManager.currentEvent.mainIndicator != IndicatorType.Communication || (npc.ID != eventManager.currentNPC) || dialogueManager.isActive) return;
        StartCoroutine(dialogueManager.Speak(eventManager.currentEvent.dialogue.spanish));
    }

    private void HandleTagAction(string objectTag, RaycastHit hit)
    {
        switch (objectTag)
        {
            case "Semillas":
                Debug.Log($"Impacto con Semillas: {hit.collider.name}");
                isCultivating = true;
                semillas.SetActive(true);
                isFree = false;
                break;

            case "Cultivo":
                Debug.Log($"Impacto con Cultivo: {hit.collider.name}");
                cultivo.SetActive(true);
                isFree = false;
                break;
        }
    }

    private void HandleCropHit(RaycastHit cropHit)
    {
        string objectTag = cropHit.collider.tag;

        if (objectTag == "Cultivo")
        {
            Crop crop = cropHit.collider.GetComponent<Crop>();
            if (crop != null)
            {
                crop.isCropFree = true;
                Debug.Log("KLK");
                GameObject collidedObject = cropHit.collider.gameObject;
                Transform childTransform = collidedObject.transform.GetChild(1);
                if (childTransform != null)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ValidateInteraction(int eventId, string objectTag, RaycastHit hit)
    {
        var actions = new Dictionary<int, (string tag, GameObject obj, string message)>
        {
            { 1, ("Tetera", tinto, "Impacto con una Tetera") },
            { 2, ("Librero", libro, "Impacto con un Librero") },
            { 3, ("Tetera", tinto, "Impacto con una Tetera 2") },
            { 4, ("Librero", libro, "Impacto con un Librero 2") }
        };

        if (actions.TryGetValue(eventId, out var action) && action.tag == objectTag)
        {
            Debug.Log($"{action.message}: {hit.collider.name}");
            action.obj.SetActive(true);
            isFree = false;
        }
    }

    private void ValidateDialog(int eventId, string objectTag, RaycastHit hit)
    {
        var actions = new Dictionary<int, (string tag, GameObject obj, string message)>
        {
            { 1, ("Tetera", tinto, "Impacto con una Tetera") },
            { 2, ("Librero", libro, "Impacto con un Librero") },
            { 3, ("Tetera", tinto, "Impacto con una Tetera 2") },
            { 4, ("Librero", libro, "Impacto con un Librero 2") }
        };

        if (actions.TryGetValue(eventId, out var action) && action.tag == objectTag)
        {
            Debug.Log($"{action.message}: {hit.collider.name}");
            action.obj.SetActive(true);
            isFree = false;
        }
    }
}
