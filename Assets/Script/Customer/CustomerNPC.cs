using UnityEngine;

public class CustomerNPC : BaseInteractable, IInteractable
{
    [SerializeField] private NPCDataSO data;

    private bool hasGivenOrder = false;
    public override void EscapeInteract(PlayerInteractor player)
    {
        
    }

    public override void Interact(PlayerInteractor player)
    {
        if (data == null)
        {
            Debug.Log("No NPC data assigned");
            return; 
        }

        CraftingStep currentStep = OrderManager.Instance.CurrentStep;

        if (currentStep == CraftingStep.Idle && !hasGivenOrder)
        {
            Debug.Log("Order received");
            GiveOrder();
            return;
        }

        // Player returns to deliver the finished sword
        if (currentStep == CraftingStep.Delivery && hasGivenOrder)
        {
            DeliverSword();
            return;
        }

        // Still crafting — customer is waiting
        if (hasGivenOrder)
        {
            Debug.Log($"{data.npcName}: \"{data.waitingDialogue}\"");
        }

    }

    private void GiveOrder()
    {
        hasGivenOrder = true;
        OrderManager.Instance.ReceiveOrder(data.swordOrder);
        Debug.Log($"{data.npcName}: \"{data.orderDialogue}\"");
    }

    private void DeliverSword()
    {
        hasGivenOrder = false;
        OrderManager.Instance.CompleteStep(CraftingStep.Delivery, 0f);
        Debug.Log($"{data.npcName}: \"{data.deliveryDialogue}\"");
    }

}
