using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject orderPanel;

    [Header("NPC Info")]
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Sword Order")]
    [SerializeField] private Image swordIcon;
    [SerializeField] private TextMeshProUGUI swordNameText;

    [Header("Step Tracker")]
    [SerializeField] private TextMeshProUGUI currentStepText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideChat();
        HideOrder();
    }

    public void ShowOrder(NPCDataSO npcData)
    {
        if(npcData == null) { return; }

        orderPanel.SetActive(true);

        SwordDataSO sword = npcData.swordOrder?.requestedSword;
        if (sword != null)
        {
            swordNameText.text = sword.swordName;
            swordIcon.sprite = sword.icon;
            swordIcon.gameObject.SetActive(sword.icon != null);
        }

        UpdateStep(CraftingStep.Smelting);


    }

    public void ShowChat(string npcName, string dialogue, Sprite portrait)
    {
        chatPanel.SetActive(true);

        npcNameText.text = npcName;
        dialogueText.text = dialogue;

        this.portrait.sprite = portrait;
        this.portrait.gameObject.SetActive(portrait != null);
    }

    public void UpdateStep(CraftingStep step)
    {
        if(currentStepText == null) { return; }

        currentStepText.text = step switch
        {
            CraftingStep.Smelting   => "Step 1: Smelt the metal",
            CraftingStep.Forging    => "Step 2: Forge the blade",
            CraftingStep.Quenching  => "Step 3: Quench the blade",
            CraftingStep.Delivery   => "Hand to customer",
            CraftingStep.Complete   => "Order complete",
            _                       => ""
        };
    }

    public void HideChat()
    {
        chatPanel.SetActive(false);
    }

    public void HideOrder()
    {
        orderPanel.SetActive(false);
    }
}
