using Unity.VisualScripting;
using UnityEngine;

public class CustomerNPC : BaseInteractable, IInteractable
{
    [SerializeField] private NPCDataSO data;
    [SerializeField] private Transform playerStayPosition;
    [SerializeField] private float trasitionDuration = 0.4f;

    [SerializeField] private bool hasGivenOrder = false;
    [SerializeField] PlayerInteractor playerInteractor;

    private void OnEnable()
    {
        hasGivenOrder = false;
    }

    public void LockInteraction()
    {
        playerInteractor.animationIsPlaying = true;
    }

    public void UnlockInteraction()
    {
        playerInteractor.animationIsPlaying = false;
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        PlayerControl control = player.GetComponent<PlayerControl>();
        if (control != null)
        {
            OrderUI.Instance.HideChat();

            if (!hasGivenOrder) { OrderUI.Instance.HideOrder(); }
        }
    }

    public override void Interact(PlayerInteractor player)
    {
        
        if (data == null)
        {
            Debug.Log("No NPC data assigned");
            return; 
        }

        CraftingStep currentStep = OrderManager.Instance.CurrentStep;
        
        PlayerControl control = player.GetComponent<PlayerControl>();
        if (control != null)
        {
            control.AlignToStation(playerStayPosition, trasitionDuration);
            OrderUI.Instance.ShowOrder(data);
            OrderUI.Instance.UpdateStep(currentStep);
            Debug.Log($"Aligning to {playerStayPosition.name} on NPC {gameObject.name}");
        }
        
        if (currentStep == CraftingStep.Idle && !hasGivenOrder)
        {
            GiveOrder();
            OrderUI.Instance.ShowChat(data.npcName, data.orderDialogue, data.portrait);
            return;
        }

        if (currentStep == CraftingStep.Delivery && hasGivenOrder)
        {
            DeliverSword();
            return;
        }

        if (hasGivenOrder)
        {
            OrderUI.Instance.ShowChat(data.npcName, data.waitingDialogue, data.portrait);
            OrderUI.Instance.UpdateStep(currentStep);
        }

        
    }

    private void GiveOrder()
    {
        hasGivenOrder = true;
        OrderManager.Instance.ReceiveOrder(data.swordOrder);
        OrderUI.Instance.ShowOrder(data);
    }

    private void DeliverSword()
    {
        hasGivenOrder = false;
        OrderManager.Instance.CompleteStep(CraftingStep.Delivery, 0f);

        float final = (OrderManager.Instance.SmeltScore + OrderManager.Instance.ForgeScore + OrderManager.Instance.QuenchScore) / 3f;
        
        if (final >= 90f) { OrderUI.Instance.ShowChat(data.npcName, data.deliveryDialogueGood, data.portrait); }
        else if (final >= 50f) { OrderUI.Instance.ShowChat(data.npcName, data.deliveryDialogueOkay, data.portrait); }
        else if (final < 50f) { OrderUI.Instance.ShowChat(data.npcName, data.deliveryDialogueBad, data.portrait); }

        OrderUI.Instance.HideOrder();
    }
}
