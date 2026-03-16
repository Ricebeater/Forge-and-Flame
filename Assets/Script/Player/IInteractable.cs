using UnityEngine;

public interface IInteractable
{
    void OnHoverEnter();

    void OnHoverExit();

    void Interact(PlayerInteractor player);

    void EscapeInteract(PlayerInteractor player);

}
