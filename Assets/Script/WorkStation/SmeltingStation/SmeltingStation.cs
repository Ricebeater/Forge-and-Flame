using UnityEngine;

public class SmeltingStation : StationBase
{
    [SerializeField] private SmeltingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        base.Interact(player);

        if (gameScript != null)
        {
            gameScript.StartMinigame();
        }
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
