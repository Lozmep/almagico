using DialogueSystem;
using UnityEngine;
using TMPro;
using Indicator;
using System.Collections;

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

    [Header("Achievement Management")]
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
        
        if (Input.GetKeyDown(KeyCode.X) && !takeItem.isFree)
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.red);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, layerMask))
            {
                NPC npc = hit.collider.GetComponent<NPC>();

                if (npc.ID == eventManager.currentNPC)
                {
                    switch (takeItem.currentItemID)
                    {
                        case 1:
                            takeItem.tinto.SetActive(false);
                            takeItem.isFree = true;
                            takeItem.currentItemID = 0;
                            StartCoroutine(dialogueManager.Speak(eventManager.currentEvent.dialogue.spanish));
                            break;

                        case 2:
                            takeItem.libro.SetActive(false);
                            takeItem.isFree = true;
                            takeItem.currentItemID = 0;
                            StartCoroutine(dialogueManager.Speak(eventManager.currentEvent.dialogue.spanish));
                            break;
                        case 3:
                            break;
                    }
                }
                else if (!dialogueManager.isActive)
                {
                    dialogueManager.IntTxt();
                }
            }

            if (Physics.Raycast(origin, direction, out RaycastHit shoot, maxDistance, layerDeliveryMask))
            {
                Delivery delivery = shoot.collider.GetComponent<Delivery>();


                if (delivery.deliveryID == takeItem.currentItemID)
                {
                    takeItem.cultivo.SetActive(false);
                    StartCoroutine(ChangeIsFree());
                    delivery.deliverySum++;

                    string newString = delivery.deliverySum.ToString();
                    sellCount.text = "Ventas:" + newString;

                    if (delivery.deliverySum == 10)
                    {
                        achievEvent.CompareValuesInChildren(0);
                    }

                }

            }
        }

    }

    private IEnumerator ChangeIsFree()
    {
        yield return new WaitForSeconds(1f);
        takeItem.currentItemID = 0;
        takeItem.isFree = true;
    }

}
