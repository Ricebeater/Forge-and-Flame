using UnityEngine;
using System.Collections;

public class QuenchingStation : StationBase
{
    [SerializeField] private QuenchingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        if(!OrderManager.Instance.IsCurrentStep(CraftingStep.Quenching))
        {
            Debug.Log("submerge this and you will face my wrath");
            return;
        }

        base.Interact(player);
        gameScript?.StartMinigame();
        
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        base.EscapeInteract(player);
        if (gameScript != null)
        {
            float score = gameScript.CalculatedScore();
            gameScript.EndMinigame();
            OrderManager.Instance.CompleteStep(CraftingStep.Quenching, score);
        }
    }
}
