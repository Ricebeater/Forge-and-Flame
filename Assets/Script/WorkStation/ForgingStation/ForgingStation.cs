using UnityEngine;
using System.Collections;

public class ForgingStation : StationBase
{
    [SerializeField] private RhythmForgingGame gameScript;

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
