using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("Order")]
    [SerializeField] private SwordOrderSO order;
    
    [Header("Player")]
    [SerializeField] private Transform playerHand;

    [Header("Events")]
    public UnityEvent<CraftingStep> onStepChange;
    public UnityEvent onOrderComplete;

    public CraftingStep CurrentStep { get; private set; } = CraftingStep.Idle;

    private SwordOrderSO currentOrder;
    private GameObject currentSword;

    //Scores
    private float smeltScore;
    private float forgeScore;
    private float quenchScore;

    public float SmeltScore => smeltScore;
    public float ForgeScore => forgeScore;
    public float QuenchScore => quenchScore;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DayManager.Instance.OnDayStart.AddListener(OnDayStart);
    }

    private void OnDayStart()
    {
        currentOrder = null;
        CurrentStep = CraftingStep.Idle;
    }

    public void ReceiveOrder(SwordOrderSO order)
    {
        if(order == null)
        {
            Debug.Log("No Order la");
            return;
        }

        currentOrder = order;
        CurrentStep = CraftingStep.TakeOrder;
        onStepChange?.Invoke(CurrentStep);

        Debug.Log($"Order received: {order.requestedSword.swordName}. Start at the smelter!");
    }

    public bool IsCurrentStep(CraftingStep step)
    {
        return CurrentStep == step;
    }

    public void CompleteStep(CraftingStep step, float score)
    {
        switch (step)
        {
            case CraftingStep.TakeOrder:
                Debug.Log("Order accepted. Heading to smelter!");
                CurrentStep = CraftingStep.Smelting;
                break;

            case CraftingStep.Smelting:
                smeltScore = score;
                Debug.Log($"Smelting done. Score: {score:F1}");
                CurrentStep = CraftingStep.Forging;
                break;

            case CraftingStep.Forging:
                forgeScore = score;
                Debug.Log($"Forging done. Score: {score:F1}");
                CurrentStep = CraftingStep.Quenching;
                break;

            case CraftingStep.Quenching:
                quenchScore = score;
                Debug.Log($"Quenching done. Score: {score:F1}");
                CurrentStep = CraftingStep.Delivery;
                break;

            case CraftingStep.Delivery:
                CurrentStep = CraftingStep.Complete;
                onOrderComplete?.Invoke();
                CurrentStep = CraftingStep.Complete;
                currentOrder = null;

                break;

        }

        onStepChange?.Invoke(CurrentStep);
        Debug.Log($"Next Step: {CurrentStep}");
    }

    public SwordOrderSO GetCurrentOrder() => currentOrder;

    
}
