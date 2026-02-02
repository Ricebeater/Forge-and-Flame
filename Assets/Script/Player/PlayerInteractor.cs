using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction interactAction;
    private InputAction escapeAction;

    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactLayerMask;

    private IInteractable currentInteractable;

    [SerializeField] private Transform orientation;

    private PlayerControl playerControl;

    #region Starting

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        escapeAction = InputSystem.actions.FindAction("Escape");
        playerControl = GetComponent<PlayerControl>();
    }
    #endregion

    private void Update()
    {
        GetLookedAtObject();

        if (interactAction.WasPressedThisFrame())
        {
            Debug.Log("Interact pressed");

            if (currentInteractable != null)
            {
                currentInteractable.OnHoverExit();

                currentInteractable.Interact(this);
            }

        }

        if (escapeAction.WasPressedThisFrame())
        {
            Debug.Log("Escape pressed");

            if (currentInteractable != null)
            {
                currentInteractable.EscapeInteract(this);
            }
        }
    }

    private void GetLookedAtObject()
    {
        if (playerControl._isWorkingAtStation) { return; }

        RaycastHit hit;
        if (Physics.Raycast(orientation.transform.position, orientation.transform.forward, out hit, interactRange, interactLayerMask))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                if (currentInteractable != interactable)
                {
                    if (currentInteractable != null)
                    {
                        currentInteractable.OnHoverExit();
                    }
                    currentInteractable = interactable;
                    currentInteractable.OnHoverEnter();
                }
                return;
            }   
        }
        if (currentInteractable != null)
        {
            currentInteractable.OnHoverExit();
            currentInteractable = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(orientation.transform.position, orientation.transform.forward * interactRange);
    }
}
