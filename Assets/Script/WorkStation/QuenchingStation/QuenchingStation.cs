using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuenchingStation : StationBase
{
    [SerializeField] private QuenchingGame gameScript;
    public override void Interact(PlayerInteractor player)
    {
        base.Interact(player);

        if (gameScript != null)
        {
            gameScript.StartMinigame();
        }
    }
}
