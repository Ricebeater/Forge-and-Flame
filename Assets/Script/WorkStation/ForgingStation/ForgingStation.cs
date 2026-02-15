using UnityEngine;

public class ForgingStation : StationBase
{
    [SerializeField] private ForgingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        base.Interact(player);
        if( gameScript != null)
        {
            gameScript.StartMiniGame();
        }
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
