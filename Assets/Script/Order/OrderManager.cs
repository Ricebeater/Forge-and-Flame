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

    public void ReceiveOrder(SwordOrderSO order)
    {
        if(order == null)
        {
            Debug.Log("No Order la");
            return;
        }

        currentOrder = order;
        CurrentStep = CraftingStep.Smelting;
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
                SpawnSwordInHand(order.requestedSword);
                onOrderComplete?.Invoke();
                LogFinalScore();

                CurrentStep = CraftingStep.Idle;
                currentOrder = null;

                break;

        }

        onStepChange?.Invoke(CurrentStep);
        Debug.Log($"Next Step: {CurrentStep}");
    }

    private void LogFinalScore()
    {
        float finalScore = (smeltScore + forgeScore + quenchScore) /3f;
        Debug.Log($"Order complete! Final Score: {finalScore:F1}%");
    }

    private void SpawnSwordInHand(SwordDataSO swordData)
    {
        if(swordData.prefab == null)
        {
            Debug.LogWarning($"Order Manager: No prefab assigned for {swordData.swordName}");
            return;
        }

        if(currentSword != null)
        {
            Destroy(currentSword);
        }

        currentSword = Instantiate(swordData.prefab, playerHand);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;

        Debug.Log($"{swordData.swordName} spawned in player's hand.");
    }

    public SwordOrderSO GetCurrentOrder() => order;
}
