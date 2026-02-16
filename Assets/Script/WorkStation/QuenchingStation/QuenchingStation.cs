using UnityEngine;
using System.Collections;

public class QuenchingStation : StationBase
{
    [SerializeField] private QuenchingGame gameScript;
    public override void Interact(PlayerInteractor player)
    {
        if (player.currentHeldItem == null)
        {
            Debug.Log("Blade Required");
            return;
        }

        if (player.currentHeldItem.state.progressStage != 2)
        {
            Debug.Log("Cant forge this");
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
        sword.state.progressStage = 3;
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
