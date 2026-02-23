using UnityEngine;
using System.Collections;

public class ForgingStation : StationBase
{
    [SerializeField] private RhythmForgingGame gameScript;
    [SerializeField] private bool bypassOrder = false;

    public override void Interact(PlayerInteractor player)
    {
        if (!OrderManager.Instance.IsCurrentStep(CraftingStep.Forging) && !bypassOrder)
        {
            Debug.Log("Do not hammer it!");
            return;
        }
        
        base.Interact(player);
        gameScript?.StartMiniGame();
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        base.EscapeInteract(player);
        
        if (gameScript != null)
        {
            float score = HitManager.Instance.CalculatedScore();
            gameScript.EndMiniGame();
            OrderManager.Instance.CompleteStep(CraftingStep.Forging, score);
        }
    }
}
