using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPreviewUI : MonoBehaviour
{
    public static OrderPreviewUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Sword Info")]
    [SerializeField] private Image swordIcon;
    [SerializeField] private TextMeshProUGUI swordNameText;
    [SerializeField] private TextMeshProUGUI swordDescriptionText;

    [Header("Button")]
    [SerializeField] private Button proceedButton;

    public event System.Action OnProceed;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        panel.SetActive(false);

        proceedButton.onClick.AddListener(OnProceedClicked);
    }

    public void ShowPreview(SwordDataSO sword)
    {
        if(sword == null) return;

        swordIcon.sprite = sword.icon;
        swordNameText.text = sword.swordName;
        swordDescriptionText.text = sword.description;
        panel.SetActive(true);
    }

    private void OnProceedClicked()
    {
        panel.SetActive(false);
        OnProceed?.Invoke();
    }

}
