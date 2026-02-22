using TMPro;
using UnityEngine;

public class StationBase : BaseInteractable
{
    [SerializeField] private Transform playerStayPosition;
    [SerializeField] private float trasitionDuration = 0.4f;

    [SerializeField] private GameObject interactPrompt;

    private void Update()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(isHover);
        }
    }
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
    }

    public override void EscapeInteract(PlayerInteractor player)
    {
        PlayerControl control = player.GetComponent<PlayerControl>();

        if (control != null)
        {
            Debug.Log("Get out from " + gameObject.name + " Station");
            control.GetOutOfStation();
        }
    }
}
