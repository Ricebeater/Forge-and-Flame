using UnityEngine;

public class StationBase : BaseInteractable
{
    [SerializeField] private Transform playerStayPosition;
    [SerializeField] private float trasitionDuration = 0.4f;

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
}
