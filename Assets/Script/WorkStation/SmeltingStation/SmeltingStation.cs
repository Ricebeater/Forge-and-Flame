using System.Collections;
using UnityEngine;

public class SmeltingStation : StationBase
{
    [SerializeField] private SmeltingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        if(player.currentHeldItem == null)
        {
            Debug.Log("Ore Required");
            return;
        }

        if (player.currentHeldItem.state.progressStage != 0)
        {
            Debug.Log("No need to smelt this");
            return;
        }

        base.Interact(player);

        if (gameScript != null)
        {
            gameScript.StartMinigame();
            StartCoroutine(WaitForMinigame(player.currentHeldItem));
        }
    }

    private IEnumerator WaitForMinigame(SwordItem sword)
    {
        while (gameScript.isMiniGameActive)
        {
            yield return null;
        }

        sword.state.smeltingScore = gameScript.CalculatedScore();
        sword.state.progressStage = 1;
        sword.UpdateVisuals();

        Debug.Log($"Smelting Complete! Score: {sword.state.smeltingScore}");
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        base.EscapeInteract(player);
        if (gameScript != null)
        {
            gameScript.EndMinigame();
        }
    }

}
