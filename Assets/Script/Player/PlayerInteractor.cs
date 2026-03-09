using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerInteractor : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction interactAction;
    private InputAction escapeAction;

    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactLayerMask;

    [Header("Stations")]
    [SerializeField] private CustomerNPC customerNPC;
    [SerializeField] private SmeltingStation smeltingStation;
    [SerializeField] private ForgingStation forgingStation;
    [SerializeField] private QuenchingStation quenchingStation;

    [SerializeField] private float deliveryDialogueDelay = 3f;
    [SerializeField] private float orderDialogDelay = 3f;

    private IInteractable currentInteractable;

    [SerializeField] private Transform orientation;

    private PlayerControl playerControl;

    private void OnEnable() => inputActions.FindActionMap("Player").Enable();
    private void OnDisable() => inputActions.FindActionMap("Player").Disable();

    private void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        escapeAction = InputSystem.actions.FindAction("Escape");
        playerControl = GetComponent<PlayerControl>();
    }

    private void Start()
    {
        OrderManager.Instance.onStepChange.AddListener(OnStepChanged);

    }

    private void Update()
    {
        DebugCurrentStep();

        HandleEscape();

        if (OrderManager.Instance.CurrentStep == CraftingStep.Idle)
        {
            HandleInteract();
        }
    }

    private void HandleInteract()
    {
        if (!interactAction.WasPressedThisFrame()) return;

        currentInteractable = customerNPC;
        customerNPC.Interact(this);
    }

    private void HandleEscape()
    {
        if (!escapeAction.WasPressedThisFrame()) return;
        if (currentInteractable == null) return;

        var leaving = currentInteractable;
        currentInteractable = null;

        leaving.EscapeInteract(this);
    }

    private void ActivateStation(StationBase station)
    {
        OrderUI.Instance?.HideChat();

        currentInteractable = station;
        station.Interact(this);
    }

    private void OnStepChanged(CraftingStep step)
    {
        switch (step)
        {
            case CraftingStep.Smelting:
                StartCoroutine(AdvanceToStationAfterDelay(smeltingStation));
                break;

            case CraftingStep.Forging:
                ActivateStation(forgingStation);
                break;

            case CraftingStep.Quenching:
                ActivateStation(quenchingStation);
                break;

            case CraftingStep.Delivery:
                StartCoroutine(SummaryThenDeliveryRoutine());
                break;

            case CraftingStep.Complete:
            case CraftingStep.Idle:
                break;
        }

    }

    private IEnumerator SummaryThenDeliveryRoutine()
    {
        playerControl.isWorkingAtStation = true;

        bool continued = false;
        SummaryUI.Instance.OnContinue += () => continued = true;
        SummaryUI.Instance.Show(
            OrderManager.Instance.SmeltScore,
            OrderManager.Instance.ForgeScore,
            OrderManager.Instance.QuenchScore
        );

        yield return new WaitUntil(() => continued);

        StartCoroutine(DeliveryRoutine());
    }

    private IEnumerator AdvanceToStationAfterDelay(StationBase station)
    {
        yield return null; // ignore first time that player press E

        yield return new WaitUntil(() =>
            Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.anyKey.wasPressedThisFrame
        );

        ActivateStation(station);
    }

    private IEnumerator DeliveryRoutine()
    {
        playerControl.isWorkingAtStation = true;

        currentInteractable = customerNPC;
        customerNPC.Interact(this);

        yield return new WaitForSeconds(deliveryDialogueDelay);

        OrderUI.Instance?.HideChat();
        playerControl.isWorkingAtStation = false;
        currentInteractable = null;
    }

    private void DebugCurrentStep()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame) { Debug.Log("Current Step: " + OrderManager.Instance.CurrentStep); }
    }
}
