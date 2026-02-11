using UnityEngine;

public class StationBase : BaseInteractable
{
    [SerializeField] private Transform playerStayPosition;
    [SerializeField] private float trasitionDuration = 0.4f;
    [SerializeField] private QuenchingGame gameScript;

    public override void Interact(PlayerInteractor player)
    {
        Debug.Log("Interacted with " + gameObject.name + " Station");

        if (playerStayPosition != null)
        {
            PlayerControl control = player.GetComponent<PlayerControl>();

            if (control != null)
            {
                control.AlignToStation(playerStayPosition, trasitionDuration);
            }
        }

        if (gameScript != null)
        {
            gameScript.StartMinigame();
        }
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        PlayerControl control = player.GetComponent<PlayerControl>();

        if (control != null)
        {
            Debug.Log("Get out from " + gameObject.name + " Station");
            control.GetOutOfStation();
        }

        if (gameScript != null)
        {
            gameScript.EndMinigame();
        }
    }
}
