using DialogueSystem;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using TMPro;
using Indicator;

public class GiveItemEvent : MonoBehaviour
{

    private TakeItemEvent takeItem;

    [Header("Raycast Info")]
    private RaycastHit hit;
    private Vector3 origin;
    private Vector3 direction;
    public float maxDistance = 1f;
    public LayerMask layerMask;
    public LayerMask layerDeliveryMask;
    public float verticalDistance = 0.9f;

    [Header("Event Management")]
    public EventManager.EventManager eventManager;

    [Header("Event Management")]
    public AchievementSystem achievEvent;

    [Header("Dialogue Management")]
    public DialogueManager dialogueManager;

    [Header("Initial Event Dialogue Management")]
    public EventDialogue eventDialogue;

    [Header("Indicator Management")]
    public IndicatorManager indicatorManager;

    [Header("Selling Features")]
    public TextMeshProUGUI sellCount;


    private void Awake()
    {
        takeItem = GetComponent<TakeItemEvent>();
        eventDialogue = GetComponent<EventDialogue>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + new Vector3(0, verticalDistance, 0);
        
        if (Input.GetKey(KeyCode.X) && !takeItem.isFree)
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.red);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                NPC npc = hit.collider.GetComponent<NPC>();
                Debug.Log($"tARGET: {eventManager.currentNPC} DETECTED: {npc.ID}");

                if (npc.ID != eventManager.currentNPC || !dialogueManager.isActive) {
                    dialogueManager.IntTxt();
                    return;
                }

                switch (takeItem.currentItemID)
                {
                    case 1:
                        Debug.Log("Gracias por el tinto! Seamos amigos");
                        takeItem.tinto.SetActive(false);
                        takeItem.isFree = true;
                        takeItem.currentItemID = 0;
                        StartCoroutine(dialogueManager.Speak(eventManager.currentEvent.dialogue.spanish));
                        eventDialogue.isIndicated = false;
                        break;

                    case 2:
                        Debug.Log("Leer es saber");
                        takeItem.libro.SetActive(false);
                        takeItem.isFree = true;
                        takeItem.currentItemID = 0;
                        StartCoroutine(dialogueManager.Speak(eventManager.currentEvent.dialogue.spanish));
                        eventDialogue.isIndicated = false;
                        break;
                    case 3:
                        Debug.Log("Gracias por la venta! ");
                        break;
                }
            }

            if (Physics.Raycast(origin, direction, out RaycastHit shoot, maxDistance, layerDeliveryMask))
            {
                Debug.Log("Entra");
                Delivery delivery = shoot.collider.GetComponent<Delivery>();


                if (delivery.deliveryID == takeItem.currentItemID)
                {
                    Debug.Log("Gracias por la venta!");
                    takeItem.cultivo.SetActive(false);
                    takeItem.currentItemID = 0;
                    takeItem.isFree = true;
                    delivery.deliverySum++;
                    indicatorManager.modifyIndicators(0f, 0f, 0f, 10f);

                    string newString = delivery.deliverySum.ToString();
                    sellCount.text = "Ventas:" + newString;

                    Debug.Log(delivery.deliverySum);

                    if (delivery.deliverySum == 1)
                    {
                        Debug.Log("Ha obtenido el primer logro");
                        achievEvent.CompareValuesInChildren(0);
                    }

                }

            }
        }

    }

}
