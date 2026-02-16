using UnityEngine;
using System.Collections;

public class ForgingStation : StationBase
{
    [SerializeField] private ForgingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        if (player.currentHeldItem == null)
        {
            Debug.Log("blade Required");
            return;
        }

        if(player.currentHeldItem.state.progressStage != 1)
        {
            Debug.Log("No need to forge this");
            return;
        }

        base.Interact(player);
        if( gameScript != null)
        {
            gameScript.StartMiniGame();
            StartCoroutine(WaitForMinigame(player.currentHeldItem));
        }
    }

    private IEnumerator WaitForMinigame(SwordItem sword)
    {
        while (gameScript.isMiniGameActive)
        {
            yield return null;
        }
        sword.state.forgingScore = gameScript.CalculatedScore();
        sword.state.progressStage = 2;
        sword.UpdateVisuals();
        Debug.Log($"Forging Complete! Score: {sword.state.forgingScore}");
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        base.EscapeInteract(player);
        if (gameScript != null)
        {
            gameScript.EndMiniGame();
        }
    }
}
