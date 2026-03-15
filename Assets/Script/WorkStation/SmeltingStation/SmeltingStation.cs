using System.Collections;
using UnityEngine;

public class SmeltingStation : StationBase
{
    [SerializeField] private SmeltingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        if (!OrderManager.Instance.IsCurrentStep(CraftingStep.Smelting))
        {
            Debug.Log("you shouldn't be smeltin' this!");
            //return;
        }

        if(DayManager.Instance.dayNumber == 1)
        {
            TutorialUI.Instance.ShowTutorial(TutorialUI.Instance.smeltingTutorial, DayManager.Instance.IsFirstDay());
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
            OrderManager.Instance.CompleteStep(CraftingStep.Smelting, score);
        }
    }

}
